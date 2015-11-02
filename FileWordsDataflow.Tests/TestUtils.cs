namespace FileWordsDataflow.Tests
{
    using System;
    using System.Threading.Tasks.Dataflow;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal static class TestUtils
    {
        public static T ReceiveWithTimeout<T>(this ISourceBlock<T> block)
        {
            return block.Receive(TimeSpan.FromMilliseconds(100));
        }

        public static void EnsureCompleted(this IDataflowBlock block)
        {
            Assert.IsTrue(block.Completion.Wait(TimeSpan.FromMilliseconds(100)));
        }
    }
}