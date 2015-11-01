namespace FileWordsDataflow.Dal
{
    using System.Collections.Generic;
    using DataModel;

    public interface IRepository
    {
        void SaveFiles(IEnumerable<File> files);
    }
}