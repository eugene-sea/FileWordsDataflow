namespace FileWordsDataflow.Tests
{
    using System.Threading.Tasks.Dataflow;
    using DataModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.DataflowBlocks;

    [TestClass]
    public class FileWordCreatorTests
    {
        [TestMethod]
        public void ShouldReturnTheSameWordInstanceForAllOccurrencesOfThisWord()
        {
            var block = FileWordCreator.GetFileWordCreatorBlock();
            block.Post(new LineSplitter.FileLineWord(new File(), "TEST", 1, 1));
            var file = new File();
            block.Post(new LineSplitter.FileLineWord(file, "TEST", 3, 14));
            var word1 = block.ReceiveWithTimeout();
            var word2 = block.ReceiveWithTimeout();
            block.Complete();
            block.EnsureCompleted();
            Assert.AreSame(word1.Word, word2.Word);
            Assert.AreEqual("TEST", word2.Word.Term);
            Assert.AreSame(file, word2.File);
            Assert.AreEqual(3, word2.Row);
            Assert.AreEqual(14, word2.Col);
        }
    }
}
