namespace FileWordsDataflow.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dal;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services;

    [TestClass]
    public class FileWordsServiceTests
    {
        [TestMethod]
        public async Task ShouldParseAllTextFilesInTestFolderAndFillDb()
        {
            var service = new FileWordsService(() => new Repository());
            await service.ParseFilesAsync("TestFolder", "*.txt");
            var firstFileWord = (await service.GetFileWordStatsAsync(skip: 0, take: 1)).Single();
            var twelfth = (await service.GetFileWordStatsAsync(skip: 11, take: 1)).Single();
            Assert.AreEqual("DO", firstFileWord.Word);
            Assert.AreEqual("TextFile2.txt", firstFileWord.File);
            Assert.AreEqual(4, firstFileWord.FirstRow);
            Assert.AreEqual(16, firstFileWord.FirstCol);
            Assert.AreEqual(1, firstFileWord.Occurrences);

            Assert.AreEqual("WINDOW", twelfth.Word);
            Assert.AreEqual("TextFile2.txt", twelfth.File);
            Assert.AreEqual(3, twelfth.FirstRow);
            Assert.AreEqual(20, twelfth.FirstCol);
            Assert.AreEqual(2, twelfth.Occurrences);
        }
    }
}