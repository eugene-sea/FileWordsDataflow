namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System;
    using System.Threading.Tasks.Dataflow;
    using Dal;
    using DataModel;

    internal static class FileSaver
    {
        public static IPropagatorBlock<File, File> GetFileSaverBlock(Func<IRepository> repositoryFactory)
        {
            var batchBlock = new BatchBlock<File>(100);
            var transformBlock = new TransformManyBlock<File[], File>(
                files =>
                {
                    var repo = repositoryFactory();
                    repo.SaveFiles(files);
                    return files;
                });
            batchBlock.LinkTo(transformBlock);
            batchBlock.Completion.ContinueWith(t => Utils.PropagateCompleted(t, transformBlock));
            return DataflowBlock.Encapsulate(batchBlock, transformBlock);
        }
    }
}