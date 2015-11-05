namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Dal;
    using DataModel;

    internal static class FileWordSaver
    {
        public static IPropagatorBlock<FileWord, FileWord> GetFileWordSaverBlock(Func<IRepository> repositoryFactory)
        {
            var inputBuffer = new BufferBlock<FileWord>();
            var outputBuffer = new BufferBlock<FileWord>();
            
            var batchBlockWithoutParents = new BatchBlock<FileWord>(1000);
            inputBuffer.LinkTo(batchBlockWithoutParents, i => i.File.FileId > 0 && i.Word.WordId > 0);
            inputBuffer.PropagateCompleted(batchBlockWithoutParents);
            var saveWithoutParentsBlock = new TransformManyBlock<FileWord[], FileWord>(
                async fileWords =>
                {
                    return await SaveEntities(repositoryFactory, fileWords, true);
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Utils.GlobalMaxDegreeOfParallelism
                });
            batchBlockWithoutParents.LinkToAndPropagateCompleted(saveWithoutParentsBlock);
            saveWithoutParentsBlock.LinkTo(outputBuffer);

            var batchBlockWithParents = new BatchBlock<FileWord>(300);

            // MaxMessages value means inputBuffer will unlink automatically from batchBlockWithParents after 300 messages.
            // This is required to prohibit "stealing" of workload from saveWithoutParentsBlock
            inputBuffer.LinkTo(batchBlockWithParents, new DataflowLinkOptions { MaxMessages = 300 });
            inputBuffer.PropagateCompleted(batchBlockWithParents);

            var saveWithParentsBlock = new TransformManyBlock<FileWord[], FileWord>(
                async fileWords =>
                {
                    var res = await SaveEntities(repositoryFactory, fileWords, false);
                    FileWord fileWord;
                    if (inputBuffer.TryReceive(out fileWord)) // This unblocks inputBuffer due to unlinking from batchBlockWithParents
                    {
                        if (fileWord.File.FileId > 0 && fileWord.Word.WordId > 0)
                        {
                            batchBlockWithoutParents.Post(fileWord);
                        }
                        else
                        {
                            batchBlockWithParents.Post(fileWord);
                        }
                    }

                    // Link again for another 300 messages
                    inputBuffer.LinkTo(batchBlockWithParents, new DataflowLinkOptions { MaxMessages = 300 });
                    return res;
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1 // If we are saving files/words also we cannot do this concurrently or duplicates of files/words may be inserted
                });
            batchBlockWithParents.LinkToAndPropagateCompleted(saveWithParentsBlock);
            saveWithParentsBlock.LinkTo(outputBuffer);

            saveWithoutParentsBlock.Completion.ContinueWith(
                t => ((IDataflowBlock)outputBuffer).Fault(t.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
            saveWithParentsBlock.Completion.ContinueWith(
                t => ((IDataflowBlock)outputBuffer).Fault(t.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
            Task.WhenAll(saveWithoutParentsBlock.Completion, saveWithParentsBlock.Completion)
                .ContinueWith(t => outputBuffer.Complete(), TaskContinuationOptions.NotOnFaulted);

            return DataflowBlock.Encapsulate(inputBuffer, outputBuffer);
        }

        private static async Task<FileWord[]> SaveEntities(
            Func<IRepository> repositoryFactory,
            FileWord[] fileWords,
            bool doNotSaveParentEntities)
        {
            var repo = repositoryFactory();

            var filesToSave =
                doNotSaveParentEntities
                    ? Enumerable.Empty<File>()
                    : fileWords.GroupBy(w => w.File).Select(g => g.Key).Where(f => f.FileId < 1).ToArray();
            var wordsToSave =
                doNotSaveParentEntities
                    ? Enumerable.Empty<Word>()
                    : fileWords.GroupBy(w => w.Word).Select(g => g.Key).Where(w => w.WordId < 1).ToArray();

            foreach (var word in fileWords)
            {
                if (word.File.FileId > 0)
                {
                    word.FileId = word.File.FileId;
                    word.File = null; // Set to null as we share those entities between repositories
                }

                if (word.Word.WordId > 0)
                {
                    word.WordId = word.Word.WordId;
                    word.Word = null; // Set to null as we share those entities between repositories
                }
            }

            await repo.SaveEntitiesAsync(fileWords, filesToSave, wordsToSave);
            return fileWords;
        }
    }
}
