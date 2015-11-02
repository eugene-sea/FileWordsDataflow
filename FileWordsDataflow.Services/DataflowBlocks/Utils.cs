namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System.Threading.Tasks.Dataflow;

    internal static class Utils
    {
        public static void PropagateCompleted(this IDataflowBlock sourceBlock, IDataflowBlock targetBlock)
        {
            sourceBlock.Completion.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    targetBlock.Complete();
                }
                else
                {
                    targetBlock.Fault(t.Exception);
                }
            });
        }
    }
}