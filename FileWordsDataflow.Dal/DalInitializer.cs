namespace FileWordsDataflow.Dal
{
    using System.Data.Entity;

    public static class DalInitializer
    {
        public static void Initialize(bool autoApplyDbMigrations)
        {
            if (!autoApplyDbMigrations)
            {
                Database.SetInitializer<FileWordsDataflowDbContext>(null);
            }
            else
            {
                Database.SetInitializer(new FileWordsDataflowDbContextInitializer());
            }
        }
    }
}
