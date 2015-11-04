namespace FileWordsDataflow.Dal.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class NewIndexTuning : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.FileWords", new[] { "WordId", "FileId" });
        }
        
        public override void Down()
        {
            DropIndex("dbo.FileWords", new[] { "WordId", "FileId" });
        }
    }
}
