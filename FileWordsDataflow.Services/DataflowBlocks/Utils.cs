namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    internal static class Utils
    {
        public static void PropagateCompleted(Task completionTask, IDataflowBlock targetBlock)
        {
            if (!completionTask.IsFaulted)
            {
                targetBlock.Complete();
            }
            else
            {
                targetBlock.Fault(completionTask.Exception);
            }
        }
    }
}