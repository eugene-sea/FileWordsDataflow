namespace FileWordsDataflow.Dal
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
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

        public async Task TruncateDataAsync()
        {
            using (var context = new FileWordsDataflowDbContext())
            {
                await context.Database.ExecuteSqlCommandAsync("DELETE FROM [FileWords]");
                await context.Database.ExecuteSqlCommandAsync("DELETE FROM [Files]");
                await context.Database.ExecuteSqlCommandAsync("DELETE FROM [Words]");
            }
        }

        public async Task<IList<FileWordStats>> GetFileWordStatsAsync(int skip, int take)
        {
            using (var context = new FileWordsDataflowDbContext())
            {
                var results =
                    from fw in context.FileWords.AsNoTracking()
                    group fw by new { fw.Word.Term, fw.FileId } into groups
                    let firstWord = groups.OrderBy(i => i.Row).ThenBy(i => i.Col).FirstOrDefault()
                    select new FileWordStats
                    {
                        Word = groups.Key.Term,
                        File = firstWord.File.Name,
                        FirstRow = firstWord.Row,
                        FirstCol = firstWord.Col,
                        Occurrences = groups.Count()
                    };

                var res = await results.OrderBy(i => i.Word).ThenBy(i => i.File).Skip(skip).Take(take).ToArrayAsync();
                return res;
            }
        }
    }
}