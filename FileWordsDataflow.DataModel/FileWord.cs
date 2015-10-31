namespace FileWordsDataflow.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FileWord
    {
        [Key]
        public int FileWordId { get; set; }

        [ForeignKey("File")]
        public int FileId { get; set; }

        public virtual File File { get; set; }

        [ForeignKey("Word")]
        public int WordId { get; set; }

        public virtual Word Word { get; set; }

        public int Col { get; set; }

        public int Row { get; set; }
    }
}
