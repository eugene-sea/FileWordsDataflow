namespace FileWordsDataflow.Tests
{
    using System;
    using System.Globalization;
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
            var block = FilesEnumerator.GetFilesEnumeratorBlock("TestFolder", "*.txt");
            var files = Enumerable.Repeat(1, 3).Select(_ => block.Receive(TimeSpan.FromMilliseconds(100))).ToList();
            block.Completion.Wait();
            Assert.IsTrue(
                files
                    .OrderBy(f => f)
                    .SequenceEqual(Enumerable.Range(1, 3)
                    .Select(i => string.Format(CultureInfo.InvariantCulture, "TextFile{0}.txt", i))));
        }
    }
}
