namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks.Dataflow;
    using Dal;
    using DataModel;

    internal static class FileWordSaver
    {
        public static IPropagatorBlock<FileWord, FileWord> GetFileWordSaverBlock(
            Func<IRepository> repositoryFactory, bool doNotSaveParentEntities)
        {
            var batchBlock = new BatchBlock<FileWord>(1000);
            var transformBlock = new TransformManyBlock<FileWord[], FileWord>(
                async fileWords =>
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
                },
                new ExecutionDataflowBlockOptions
                { // If we are saving files/words also we cannot do this concurrently or duplicates of files/words may be inserted
                    MaxDegreeOfParallelism = !doNotSaveParentEntities ? 1 : Utils.GlobalMaxDegreeOfParallelism
                });

            batchBlock.LinkTo(transformBlock);
            batchBlock.PropagateCompleted(transformBlock);
            return DataflowBlock.Encapsulate(batchBlock, transformBlock);
        }
    }
}