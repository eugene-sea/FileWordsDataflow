namespace FileWordsDataflow.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FileWord
    {
        [Key]
        public int FileWordId { get; set; }

        [ForeignKey("File")]
        [Index("IX_FileId")]
        [Index("IX_WordId_FileId_Row_Col", 2, IsUnique = true)]
        public int FileId { get; set; }

        public virtual File File { get; set; }

        [ForeignKey("Word")]
        [Index("IX_WordId")]
        [Index("IX_WordId_FileId_Row_Col", 1, IsUnique = true)]
        public int WordId { get; set; }

        public virtual Word Word { get; set; }

        [Index("IX_WordId_FileId_Row_Col", 3, IsUnique = true)]
        public int Row { get; set; }

        [Index("IX_WordId_FileId_Row_Col", 4, IsUnique = true)]
        public int Col { get; set; }
    }
}