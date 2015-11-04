namespace FileWordsDataflow.Services
{
    using System;
    using Common;

    public class ModuleInit : IModule
    {
        public void Initialize(ApplicationParameters parameters, IModuleRegistrar registrar)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (registrar == null)
            {
                throw new ArgumentNullException("registrar");
            }

            registrar.RegisterType<IFileWordsService, FileWordsService>();
        }
    }
}