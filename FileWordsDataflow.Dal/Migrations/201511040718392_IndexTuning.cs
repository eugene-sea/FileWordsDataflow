namespace FileWordsDataflow.Dal.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class IndexTuning : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FileWords", new[] { "FileId", "WordId", "Row", "Col" });
            CreateIndex("dbo.FileWords", new[] { "WordId", "FileId", "Row", "Col" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.FileWords", new[] { "WordId", "FileId", "Row", "Col" });
            CreateIndex("dbo.FileWords", new[] { "FileId", "WordId", "Row", "Col" }, unique: true);
        }
    }
}