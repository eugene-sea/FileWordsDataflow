namespace FileWordsDataflow.Dal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataModel;

    public interface IRepository
    {
        Task SaveEntitiesAsync(IEnumerable<FileWord> fileWords, IEnumerable<File> files, IEnumerable<Word> words);
    }
}