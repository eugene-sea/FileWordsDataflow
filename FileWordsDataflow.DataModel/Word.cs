namespace FileWordsDataflow.DataModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Word
    {
        [Key]
        public int WordId { get; set; }
        
        [Required]
        [MaxLength(450)]
        [Index("IX_Term", 1, IsUnique = true)]
        public string Term { get; set; }
    }
}
