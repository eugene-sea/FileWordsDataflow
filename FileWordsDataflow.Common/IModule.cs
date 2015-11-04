namespace FileWordsDataflow.Common
{
    public interface IModule
    {
        void Initialize(ApplicationParameters parameters, IModuleRegistrar registrar);
    }
}
