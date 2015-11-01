namespace FileWordsDataflow.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class File
    {
        [Key]
        public int FileId { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string FullPath { get; set; }
    }
}
