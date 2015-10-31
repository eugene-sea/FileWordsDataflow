namespace FileWordsDataflow.Dal.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedIndexes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FileWords", new[] { "FileId" });
            DropIndex("dbo.FileWords", new[] { "WordId" });
            AlterColumn("dbo.Words", "Term", c => c.String(nullable: false, maxLength: 450));
            CreateIndex("dbo.FileWords", "FileId");
            CreateIndex("dbo.FileWords", new[] { "FileId", "WordId", "Row", "Col" }, unique: true);
            CreateIndex("dbo.FileWords", "WordId");
            CreateIndex("dbo.Words", "Term", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Words", new[] { "Term" });
            DropIndex("dbo.FileWords", new[] { "WordId" });
            DropIndex("dbo.FileWords", new[] { "FileId", "WordId", "Row", "Col" });
            DropIndex("dbo.FileWords", new[] { "FileId" });
            AlterColumn("dbo.Words", "Term", c => c.String(nullable: false));
            CreateIndex("dbo.FileWords", "WordId");
            CreateIndex("dbo.FileWords", "FileId");
        }
    }
}
