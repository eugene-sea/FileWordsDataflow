namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System.Threading.Tasks.Dataflow;
    using DataModel;

    internal static class FileLinesEnumerator
    {
        public static IPropagatorBlock<File, FileLine> GetFileLinesEnumeratorBlock()
        {
            var resultsBlock = new BufferBlock<FileLine>();
            var actionBlock = new ActionBlock<File>(
                async file =>
                {
                    using (var reader = new System.IO.StreamReader(new System.IO.FileStream(
                        file.FullPath,
                        System.IO.FileMode.Open, 
                        System.IO.FileAccess.Read, 
                        System.IO.FileShare.Read, 
                        bufferSize: 4096,
                        useAsync: true)))
                    {
                        string line;
                        var row = 1;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                resultsBlock.Post(new FileLine(file, row, line));
                            }

                            row++;
                        }
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded });

            actionBlock.PropagateCompleted(resultsBlock);
            return DataflowBlock.Encapsulate(actionBlock, resultsBlock);
        }

        public struct FileLine
        {
            private readonly File file;
            private readonly int row;
            private readonly string line;

            public FileLine(File file, int row, string line)
            {
                this.row = row;
                this.line = line;
                this.file = file;
            }

            public File File
            {
                get { return file; }
            }

            public int Row
            {
                get { return row; }
            }

            public string Line
            {
                get { return line; }
            }
        }
    }
}