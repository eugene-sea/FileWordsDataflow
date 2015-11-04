namespace FileWordsDataflow.Services
{
    using System;
    using System.ComponentModel.Composition;
    using Common;

    [Export(typeof(IModule))]
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