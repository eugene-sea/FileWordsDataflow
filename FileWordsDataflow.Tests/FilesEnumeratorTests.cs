namespace FileWordsDataflow.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks.Dataflow;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.DataflowBlocks;

    [TestClass]
    public class FilesEnumeratorTests
    {
        [TestMethod]
        public void ShouldEnumerateTextFilesInAllSubfolders()
        {
            var block = FilesEnumerator.GetFilesEnumeratorBlock();
            block.Post(new FilesEnumerator.EnumerateFolderTask { Folder = "TestFolder", SearchPattern = "*.txt" });
            block.Complete();
            var files = Enumerable.Repeat(1, 3).Select(_ => block.Receive(TimeSpan.FromMilliseconds(100))).ToList();
            block.EnsureCompleted();
            Assert.IsTrue(
                files.OrderBy(f => f).SequenceEqual(new[]
                {
                    @"TestFolder\Subfolder\TextFile2.txt",
                    @"TestFolder\Subfolder\TextFile3.txt",
                    @"TestFolder\TextFile1.txt"
                }));
        }

        [TestMethod]
        public void ShouldFailIfFolderDoesNotExist()
        {
            try
            {
                var block = FilesEnumerator.GetFilesEnumeratorBlock();
                block.Post(new FilesEnumerator.EnumerateFolderTask
                {
                    Folder = "UnexistingFolder",
                    SearchPattern = "*.txt"
                });
                block.EnsureCompleted();
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.GetBaseException(), typeof(DirectoryNotFoundException));
            }
        }
    }
}
