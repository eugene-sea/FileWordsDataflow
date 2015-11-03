namespace FileWordsDataflow.Dal
{
    using System.Collections.Generic;
    using DataModel;

    public interface IRepository
    {
        void SaveEntities(IEnumerable<FileWord> fileWords, IEnumerable<File> files, IEnumerable<Word> words);
    }
}