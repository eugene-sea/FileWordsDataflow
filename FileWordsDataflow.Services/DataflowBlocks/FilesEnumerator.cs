namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System.IO;
    using System.Threading.Tasks.Dataflow;

    internal static class FilesEnumerator
    {
        public static IPropagatorBlock<EnumerateFolderTask, string> GetFilesEnumeratorBlock()
        {
            var resultsBlock = new BufferBlock<string>();
            var actionBlock = new ActionBlock<EnumerateFolderTask>(
                t =>
                {
                    foreach (
                        var file in Directory.EnumerateFiles(t.Folder, t.SearchPattern, SearchOption.AllDirectories))
                    {
                        resultsBlock.Post(file);
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded });

            actionBlock.PropagateCompleted(resultsBlock);
            return DataflowBlock.Encapsulate(actionBlock, resultsBlock);
        }

        public class EnumerateFolderTask
        {
            public string Folder { get; set; }

            public string SearchPattern { get; set; }
        }
    }
}