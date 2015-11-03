namespace FileWordsDataflow.Dal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataModel;

    internal class Repository : IRepository
    {
        public async Task SaveEntitiesAsync(
            IEnumerable<FileWord> fileWords, 
            IEnumerable<File> files,
            IEnumerable<Word> words)
        {
            using (var context = new FileWordsDataflowDbContext())
            {
                context.Files.AddRange(files);
                context.Words.AddRange(words);
                context.FileWords.AddRange(fileWords);
                await context.SaveChangesAsync();
            }
        }
    }
}