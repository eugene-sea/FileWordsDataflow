namespace FileWordsDataflow.DataModel
{
    public class FileWordStats
    {
        public string Word { get; set; }

        public string File { get; set; }

        public int FirstRow { get; set; }

        public int FirstCol { get; set; }

        public int Occurrences { get; set; }
    }
}