namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System.Threading.Tasks.Dataflow;
    using DataModel;

    internal static class FileCreator
    {
        public static IPropagatorBlock<string, File> GetFileCreatorBlock()
        {
            return
                new TransformBlock<string, File>(
                    filePath =>
                    {
                        filePath = System.IO.Path.GetFullPath(filePath);
                        return new File
                        {
                            Name = System.IO.Path.GetFileName(filePath),
                            Path = System.IO.Path.GetDirectoryName(filePath),
                            FullPath = filePath
                        };
                    },
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Utils.GlobalMaxDegreeOfParallelism });
        }
    }
}
