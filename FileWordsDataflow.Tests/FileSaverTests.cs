namespace FileWordsDataflow.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks.Dataflow;
    using Dal;
    using DataModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Services.DataflowBlocks;

    [TestClass]
    public class FileSaverTests
    {
        [TestMethod]
        public void ShouldSaveFilesViaRepository()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(i => i.SaveFiles(It.IsAny<IEnumerable<File>>())).Callback<IEnumerable<File>>(e =>
            {
                var counter = 1;
                foreach (var file in e)
                {
                    file.FileId = counter++;
                }
            });
            var block = FileSaver.GetFileSaverBlock(() => mock.Object);
            block.Post(new File { Name = "TestFile1.txt" });
            block.Post(new File { Name = "TestFile2.txt" });
            block.Complete();
            Assert.AreEqual(1, block.ReceiveWithTimeout().FileId);
            Assert.AreEqual(2, block.ReceiveWithTimeout().FileId);
            block.EnsureCompleted();
        }

        [TestMethod]
        public void ShouldFaultIfRepositoryRaisesException()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(i => i.SaveFiles(It.IsAny<IEnumerable<File>>())).Callback<IEnumerable<File>>(e =>
            {
                throw new InvalidOperationException();
            });

            var block = FileSaver.GetFileSaverBlock(() => mock.Object);
            block.Post(new File { Name = "TestFile1.txt" });
            block.Complete();
            try
            {
                block.EnsureCompleted();
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.GetBaseException(), typeof(InvalidOperationException));
            }
        }
    }
}