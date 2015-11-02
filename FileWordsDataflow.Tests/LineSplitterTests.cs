namespace FileWordsDataflow.Tests
{
    using System.Linq;
    using System.Threading.Tasks.Dataflow;
    using DataModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.DataflowBlocks;

    [TestClass]
    public class LineSplitterTests
    {
        [TestMethod]
        public void ShouldReturnWordAndItsPositionIfStringContainsSingleWordAndWhiteSpace()
        {
            var block = LineSplitter.GetLineSplitterBlock();
            var file = new File();
            block.Post(new FileLinesEnumerator.FileLine(file, 2, "   WORD "));
            var lineWord = block.ReceiveWithTimeout();
            block.Complete();
            block.EnsureCompleted();
            Assert.AreSame(file, lineWord.File);
            Assert.AreEqual(2, lineWord.Row);
            Assert.AreEqual(4, lineWord.Col);
            Assert.AreEqual("WORD", lineWord.Word);
        }

        [TestMethod]
        public void ShouldReturnAllWordsWithPositionsFromInputString()
        {
            var block = LineSplitter.GetLineSplitterBlock();
            block.Post(new FileLinesEnumerator.FileLine(new File(), 1, "МАМА МЫЛА РАМУ"));
            var words = Enumerable.Repeat(1, 3).Select(_ => block.ReceiveWithTimeout()).ToList();
            block.Complete();
            block.EnsureCompleted();
            Assert.IsTrue(words.Select(w => w.Col).SequenceEqual(new[] { 1, 6, 11 }));
            Assert.IsTrue(words.Select(w => w.Word).SequenceEqual(new[] { "МАМА", "МЫЛА", "РАМУ" }));
        }

        [TestMethod]
        public void ShouldReturnWordsInUppercase()
        {
            var block = LineSplitter.GetLineSplitterBlock();
            block.Post(new FileLinesEnumerator.FileLine(new File(), 1, "Word"));
            var lineWord = block.ReceiveWithTimeout();
            block.Complete();
            block.EnsureCompleted();
            Assert.AreEqual("WORD", lineWord.Word);
        }

        [TestMethod]
        public void ShouldReturnWordsSeparatedByNonwhiteSpaceCharacters()
        {
            var block = LineSplitter.GetLineSplitterBlock();
            block.Post(new FileLinesEnumerator.FileLine(new File(), 1, "TEST1NAME"));
            var words = Enumerable.Repeat(1, 2).Select(_ => block.ReceiveWithTimeout()).ToList();
            block.Complete();
            block.EnsureCompleted();
            Assert.IsTrue(words.Select(w => w.Word).SequenceEqual(new[] { "TEST", "NAME" }));
        }
    }
}