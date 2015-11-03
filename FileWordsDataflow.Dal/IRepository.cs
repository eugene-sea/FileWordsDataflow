namespace FileWordsDataflow.Dal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataModel;

    public interface IRepository
    {
        Task SaveEntitiesAsync(IEnumerable<FileWord> fileWords, IEnumerable<File> files, IEnumerable<Word> words);
        
        Task TruncateDataAsync();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "It is OK")]
        Task<IList<FileWordStats>> GetFileWordStatsAsync(int skip, int take);
    }
}