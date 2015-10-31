namespace FileWordsDataflow.DataModel
{
    using System.ComponentModel.DataAnnotations;

    public class File
    {
        [Key]
        public int FileId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
