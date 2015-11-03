namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks.Dataflow;
    using DataModel;

    internal static class FileWordCreator
    {
        public static IPropagatorBlock<LineSplitter.FileLineWord, FileWord> GetFileWordCreatorBlock()
        {
            var wordCache = new ConcurrentDictionary<string, Word>();
            var resultsBlock = new BufferBlock<FileWord>();
            var actionBlock = new ActionBlock<LineSplitter.FileLineWord>(
                w =>
                {
                   var word = wordCache.GetOrAdd(w.Word, term => new Word { Term = term });
                    resultsBlock.Post(new FileWord { File = w.File, Word = word, Row = w.Row, Col = w.Col });
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Utils.GlobalMaxDegreeOfParallelism });

            actionBlock.PropagateCompleted(resultsBlock);
            return DataflowBlock.Encapsulate(actionBlock, resultsBlock);
        } 
    }
}