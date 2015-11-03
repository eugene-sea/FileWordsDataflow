namespace FileWordsDataflow.Tests
{
    using System;
    using System.Threading.Tasks.Dataflow;
    using DataModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.DataflowBlocks;

    [TestClass]
    public class FileLinesEnumeratorTests
    {
        [TestMethod]
        public void ShouldEnumerateLinesOfFile()
        {
            var block = FileLinesEnumerator.GetFileLinesEnumeratorBlock();
            var file = new File { FullPath = @"TestFolder\TextFile1.txt" };
            block.Post(file);
            var line = block.ReceiveWithTimeout();
            block.Complete();
            block.EnsureCompleted();
            Assert.AreSame(file, line.File);
            Assert.AreEqual(1, line.Row);
            Assert.AreEqual("Мама мыла раму", line.Line);
        }

        [TestMethod]
        public void ShouldThrowFileNotFoundExceptionWhenFileIsNotFound()
        {
            var block = FileLinesEnumerator.GetFileLinesEnumeratorBlock();
            var file = new File { FullPath = @"TestFolder\UnkownFile.txt" };
            block.Post(file);
            try
            {
                block.EnsureCompleted();
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.GetBaseException(), typeof(System.IO.FileNotFoundException));
            }
        }

        [TestMethod]
        public void ShouldSkipEmptyLinesOfFile()
        {
            var block = FileLinesEnumerator.GetFileLinesEnumeratorBlock();
            var file = new File { FullPath = @"TestFolder\Subfolder\TextFile2.txt" };
            block.Post(file);
            block.ReceiveWithTimeout();
            var line = block.ReceiveWithTimeout();
            block.ReceiveWithTimeout();
            block.Complete();
            block.EnsureCompleted();
            Assert.AreSame(file, line.File);
            Assert.AreEqual(3, line.Row);
            Assert.AreEqual("Mother was washing window frame", line.Line);
        }
    }
}
