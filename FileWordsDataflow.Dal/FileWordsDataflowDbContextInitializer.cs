namespace FileWordsDataflow.Dal
{
    using System.Data.Entity;
    using Migrations;

    internal class FileWordsDataflowDbContextInitializer
        : MigrateDatabaseToLatestVersion<FileWordsDataflowDbContext, Configuration>
    {
    }
}
