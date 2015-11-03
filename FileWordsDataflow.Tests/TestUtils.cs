namespace FileWordsDataflow.Tests
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks.Dataflow;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.DataflowBlocks;

    internal static class TestUtils
    {
        static TestUtils()
        {
            Utils.GlobalMaxDegreeOfParallelism = 1;
        }

        public static T ReceiveWithTimeout<T>(this ISourceBlock<T> block)
        {
            return Debugger.IsAttached ? block.Receive() : block.Receive(TimeSpan.FromMilliseconds(100));
        }

        public static void EnsureCompleted(this IDataflowBlock block)
        {
            if (Debugger.IsAttached)
            {
                block.Completion.Wait();
                return;
            }

            Assert.IsTrue(block.Completion.Wait(TimeSpan.FromMilliseconds(500)));
        }
    }
}