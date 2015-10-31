namespace FileWordsDataflow.Dal
{
    using System.Data.Entity;
    using DataModel;

    internal class FileWordsDataflowDbContext : DbContext
    {
        public DbSet<File> Files { get; set; }

        public DbSet<Word> Words { get; set; }

        public DbSet<FileWord> FileWords { get; set; }
    }
}
