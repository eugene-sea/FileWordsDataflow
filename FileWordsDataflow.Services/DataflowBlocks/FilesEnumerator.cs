namespace FileWordsDataflow.Services.DataflowBlocks
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    internal static class FilesEnumerator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is rethrown implicitly")]
        public static ISourceBlock<string> GetFilesEnumeratorBlock(string path, string searchPattern)
        {
            var res = new BufferBlock<string>();
            Task.Run(() =>
            {
                try
                {
                    foreach (var file in Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories))
                    {
                        res.Post(file);
                    }

                    res.Complete();
                }
                catch (Exception ex)
                {
                    ((IDataflowBlock)res).Fault(ex);
                }
            });
            return res;
        }
    }
}
