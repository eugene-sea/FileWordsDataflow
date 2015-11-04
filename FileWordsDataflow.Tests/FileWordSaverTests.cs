namespace FileWordsDataflow.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Dal;
    using DataModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Services.DataflowBlocks;

    [TestClass]
    public class FileWordSaverTests
    {
        [TestMethod]
        public void ShouldSaveFileWordWithFileAndWordWhichDoNotHaveIdYetViaRepository()
        {
            var mock = new Mock<IRepository>(MockBehavior.Strict);
            mock.Setup(
                i =>
                    i.SaveEntitiesAsync(
                        It.Is<IEnumerable<FileWord>>(
                            a =>
                                a.Count() == 2 &&
                                a.All(
                                    w =>
                                        (w.FileId < 1 && w.File != null && w.File.FileId == 0) ||
                                        (w.FileId > 0 && w.File == null)) &&
                                a.All(
                                    w =>
                                        (w.WordId < 1 && w.Word != null && w.Word.WordId == 0) ||
                                        (w.WordId > 0 && w.Word == null))),
                        It.Is<IEnumerable<File>>(a => a.Single().FileId < 1),
                        It.Is<IEnumerable<Word>>(a => a.Single().WordId < 1))).Returns(Task.FromResult(true));

            var block = FileWordSaver.GetFileWordSaverBlock(() => mock.Object);
            block.Post(new FileWord { File = new File { FileId = 1 }, Word = new Word { Term = "TEST" } });
            block.Post(new FileWord { File = new File(), Word = new Word { WordId = 1, Term = "WORD" } });
            block.Complete();
            block.ReceiveWithTimeout();
            block.ReceiveWithTimeout();
            block.EnsureCompleted();
        }

        [TestMethod]
        public void ShouldFaultIfRepositoryRaisesException()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(
                i =>
                    i.SaveEntitiesAsync(
                        It.IsAny<IEnumerable<FileWord>>(),
                        It.IsAny<IEnumerable<File>>(),
                        It.IsAny<IEnumerable<Word>>())).Throws<InvalidOperationException>();

            var block = FileWordSaver.GetFileWordSaverBlock(() => mock.Object);
            block.Post(new FileWord { File = new File(), Word = new Word() });
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

        [TestMethod]
        public void ShouldNotSaveParentEntitiesIfTheyHaveIds()
        {
            var mock = new Mock<IRepository>(MockBehavior.Strict);
            mock.Setup(
                i =>
                    i.SaveEntitiesAsync(
                        It.Is<IEnumerable<FileWord>>(a => a.Count() == 2),
                        It.Is<IEnumerable<File>>(a => !a.Any()),
                        It.Is<IEnumerable<Word>>(a => !a.Any()))).Returns(Task.FromResult(true));

            var block = FileWordSaver.GetFileWordSaverBlock(() => mock.Object);
            block.Post(new FileWord { File = new File { FileId = 1 }, Word = new Word { WordId = 1 } });
            block.Post(new FileWord { File = new File { FileId = 2 }, Word = new Word { WordId = 1 } });
            block.Complete();
            block.ReceiveWithTimeout();
            block.ReceiveWithTimeout();
            block.EnsureCompleted();
        }
    }
}