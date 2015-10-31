namespace FileWordsDataflow.DataModel
{
    using System.ComponentModel.DataAnnotations;

    public class Word
    {
        [Key]
        public int WordId { get; set; }
        
        public string Term { get; set; }
    }
}
