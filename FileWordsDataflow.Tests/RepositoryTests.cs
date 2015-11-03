namespace FileWordsDataflow.Tests
{
    using System;
    using System.Threading.Tasks;
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
        public async Task ShouldFillIdsOfSavedEntities()
        {
            var repo = new Repository();
            var file = new File { Path = "TestPath", Name = "TestFile1.txt" };
            var word = new Word { Term = Guid.NewGuid().ToString() };
            var fileWord = new FileWord { File = file, Word = word };
            await repo.SaveEntitiesAsync(new[] { fileWord }, new[] { file }, new[] { word });
            Assert.IsTrue(file.FileId > 0);
            Assert.IsTrue(word.WordId > 0);
            Assert.IsTrue(fileWord.FileWordId > 0 && fileWord.FileId > 0 && fileWord.WordId > 0);
        }
    }
}