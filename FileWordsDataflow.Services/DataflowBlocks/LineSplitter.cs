namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System.Threading.Tasks.Dataflow;
    using DataModel;

    internal static class LineSplitter
    {
        public static IPropagatorBlock<FileLinesEnumerator.FileLine, FileLineWord> GetLineSplitterBlock()
        {
            var resultsBlock = new BufferBlock<FileLineWord>();
            var actionBlock = new ActionBlock<FileLinesEnumerator.FileLine>(
                l =>
                {
                    int? wordStart = null;
                    var endOfProcesssing = false;
                    for (var col = 1; !endOfProcesssing; ++col)
                    {
                        endOfProcesssing = col > l.Line.Length;
                        var ch = endOfProcesssing ? ' ' : l.Line[col - 1];
                        if (char.IsLetter(ch))
                        {
                            if (!wordStart.HasValue)
                            {
                                wordStart = col;
                            }
                        }
                        else if (wordStart.HasValue)
                        {
                            resultsBlock.Post(new FileLineWord(
                                l.File,
                                l.Line.Substring(wordStart.Value - 1, col - wordStart.Value).ToUpperInvariant(), 
                                l.Row, 
                                wordStart.Value));
                            wordStart = null;
                        }
                    }
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded });

            actionBlock.PropagateCompleted(resultsBlock);
            return DataflowBlock.Encapsulate(actionBlock, resultsBlock);
        }

        public struct FileLineWord
        {
            private readonly File file;
            private readonly string word;
            private readonly int row;
            private readonly int col;

            public FileLineWord(File file, string word, int row, int col)
            {
                this.row = row;
                this.word = word;
                this.file = file;
                this.col = col;
            }

            public File File
            {
                get { return file; }
            }

            public string Word
            {
                get { return word; }
            }

            public int Row
            {
                get { return row; }
            }

            public int Col
            {
                get { return col; }
            }
        }
    }
}