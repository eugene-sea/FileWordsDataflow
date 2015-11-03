namespace FileWordsDataflow.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataModel;

    public interface IFileWordsService
    {
        Task ParseFilesAsync(string folderPath, string searchPattern);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "It is OK")]
        Task<IList<FileWordStats>> GetFileWordStatsAsync(int skip, int take);
    }
}