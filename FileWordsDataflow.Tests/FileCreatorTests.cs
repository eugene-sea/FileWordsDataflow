namespace FileWordsDataflow.Tests
{
    using System.Threading.Tasks.Dataflow;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.DataflowBlocks;

    [TestClass]
    public class FileCreatorTests
    {
        [TestMethod]
        public void ShouldCorrectlyParseFileNameAndPath()
        {
            var block = FileCreator.GetFileCreatorBlock();
            block.Post(@"C:\TestFolder\TestFile.txt");
            var file = block.ReceiveWithTimeout();
            block.Complete();
            block.EnsureCompleted();
            Assert.AreEqual("TestFile.txt", file.Name);
            Assert.AreEqual(@"C:\TestFolder", file.Path);
        }

        [TestMethod]
        public void ShouldRelativePathsMadeAsAbsolute()
        {
            var block = FileCreator.GetFileCreatorBlock();
            block.Post(@"TestFolder\TestFile.txt");
            var file = block.ReceiveWithTimeout();
            block.Complete();
            block.EnsureCompleted();
            Assert.AreEqual("TestFile.txt", file.Name);
            Assert.IsTrue(System.IO.Path.IsPathRooted(file.Path));
            Assert.IsTrue(System.IO.Path.IsPathRooted(file.FullPath));
        }
    }
}
