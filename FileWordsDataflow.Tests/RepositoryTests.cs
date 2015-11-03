namespace FileWordsDataflow.Tests
{
    using System;
    using System.Linq;
    using System.Transactions;
    using Dal;
    using DataModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "It is OK for test class")]
    public class RepositoryTests
    {
        private TransactionScope scope;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                });
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            if (scope != null)
            {
                scope.Dispose();
                scope = null;
            }
        }

        [TestMethod]
        public void ShouldFillIdsOfSavedEntities()
        {
            var repo = new Repository();
            var file = new File { Path = "TestPath", Name = "TestFile1.txt" };
            var word = new Word { Term = Guid.NewGuid().ToString() };
            var fileWord = new FileWord { File = file, Word = word };
            repo.SaveEntitiesAsync(new[] { fileWord }, new[] { file }, new[] { word }).Wait();
            Assert.IsTrue(file.FileId > 0);
            Assert.IsTrue(word.WordId > 0);
            Assert.IsTrue(fileWord.FileWordId > 0 && fileWord.FileId > 0 && fileWord.WordId > 0);
        }

        [TestMethod]
        public void ShouldTruncateAllDataOnRequest()
        {
            var repo = new Repository();
            var file = new File { Path = "TestPath", Name = "TestFile1.txt" };
            var word = new Word { Term = Guid.NewGuid().ToString() };
            var fileWord = new FileWord { File = file, Word = word };
            repo.SaveEntitiesAsync(new[] { fileWord }, new[] { file }, new[] { word }).Wait();
            using (var context = new FileWordsDataflowDbContext())
            {
                Assert.IsTrue(context.Files.Any() && context.Words.Any() && context.FileWords.Any());
            }

            scope.Dispose();
            scope = null;
            repo.TruncateDataAsync().Wait();
            using (var context = new FileWordsDataflowDbContext())
            {
                Assert.IsTrue(!context.Files.Any() && !context.Words.Any() && !context.FileWords.Any());
            }
        }

        [TestMethod]
        public void ShouldReturnCorrectFileWordStatsAndInOrder()
        {
            var repo = new Repository();
            scope.Dispose();
            scope = null;
            repo.TruncateDataAsync().Wait();

            var file = new File { Path = "TestPath", Name = "TestFile1.txt" };
            var wordA = new Word { Term = "A" };
            var fileWordA = new FileWord { File = file, Word = wordA, Row = 3 };
            var wordB = new Word { Term = "B" };
            var fileWordB1 = new FileWord { File = file, Word = wordB, Row = 1 };
            var fileWordB2 = new FileWord { File = file, Word = wordB, Row = 2 };
            repo.SaveEntitiesAsync(
                new[] { fileWordB2, fileWordB1, fileWordA }, new[] { file }, new[] { wordB, wordA }).Wait();

            var stats = repo.GetFileWordStatsAsync(0, 2).Result;
            Assert.AreEqual("A", stats[0].Word);
            Assert.AreEqual(1, stats[0].Occurrences);

            Assert.AreEqual("B", stats[1].Word);
            Assert.AreEqual(2, stats[1].Occurrences);
            Assert.AreEqual(1, stats[1].FirstRow);
        }
    }
}