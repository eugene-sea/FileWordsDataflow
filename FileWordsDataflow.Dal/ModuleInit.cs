namespace FileWordsDataflow.Dal
{
    using System;
    using System.ComponentModel.Composition;
    using System.Data.Entity;
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

            if (!parameters.AutoApplyDbMigrations)
            {
                Database.SetInitializer<FileWordsDataflowDbContext>(null);
            }
            else
            {
                Database.SetInitializer(new FileWordsDataflowDbContextInitializer());
            }

            registrar.RegisterType<IRepository, Repository>();
        }
    }
}