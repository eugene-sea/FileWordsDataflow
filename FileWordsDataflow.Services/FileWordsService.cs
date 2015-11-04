namespace FileWordsDataflow.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Dal;
    using DataflowBlocks;
    using DataModel;

    internal class FileWordsService : IFileWordsService
    {
        private readonly Func<IRepository> repositoryFactory;

        public FileWordsService(Func<IRepository> repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public async Task ParseFilesAsync(string folderPath, string searchPattern)
        {
            var repository = repositoryFactory();
            await repository.TruncateDataAsync();

            var filesEnumerator = FilesEnumerator.GetFilesEnumeratorBlock();
            var fileCreator = FileCreator.GetFileCreatorBlock();
            filesEnumerator.LinkToAndPropagateCompleted(fileCreator);

            var fileLinesEnumerator = FileLinesEnumerator.GetFileLinesEnumeratorBlock();
            fileCreator.LinkToAndPropagateCompleted(fileLinesEnumerator);

            var lineSplitter = LineSplitter.GetLineSplitterBlock();
            fileLinesEnumerator.LinkToAndPropagateCompleted(lineSplitter);

            var fileWordCreator = FileWordCreator.GetFileWordCreatorBlock();
            lineSplitter.LinkToAndPropagateCompleted(fileWordCreator);

            var fileWordSaver = FileWordSaver.GetFileWordSaverBlock(repositoryFactory);
            fileWordCreator.LinkToAndPropagateCompleted(fileWordSaver);

            var nullTarget = DataflowBlock.NullTarget<FileWord>();
            fileWordSaver.LinkTo(nullTarget);

            filesEnumerator.Post(new FilesEnumerator.EnumerateFolderTask
            {
                Folder = folderPath,
                SearchPattern = searchPattern
            });
            filesEnumerator.Complete();
            await fileWordSaver.Completion;
        }

        public Task<IList<FileWordStats>> GetFileWordStatsAsync(int skip, int take)
        {
            return repositoryFactory().GetFileWordStatsAsync(skip, take);
        }
    }
}