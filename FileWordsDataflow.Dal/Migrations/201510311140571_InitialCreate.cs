namespace FileWordsDataflow.Dal.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Path = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FileId);
            
            CreateTable(
                "dbo.FileWords",
                c => new
                    {
                        FileWordId = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        WordId = c.Int(nullable: false),
                        Col = c.Int(nullable: false),
                        Row = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileWordId)
                .ForeignKey("dbo.Files", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.Words", t => t.WordId, cascadeDelete: true)
                .Index(t => t.FileId)
                .Index(t => t.WordId);
            
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        WordId = c.Int(nullable: false, identity: true),
                        Term = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.WordId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileWords", "WordId", "dbo.Words");
            DropForeignKey("dbo.FileWords", "FileId", "dbo.Files");
            DropIndex("dbo.FileWords", new[] { "WordId" });
            DropIndex("dbo.FileWords", new[] { "FileId" });
            DropTable("dbo.Words");
            DropTable("dbo.FileWords");
            DropTable("dbo.Files");
        }
    }
}
