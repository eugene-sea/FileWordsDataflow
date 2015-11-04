namespace FileWordsDataflow.Common
{
    using Microsoft.Practices.Unity;

    internal class ModuleRegistrar : IModuleRegistrar
    {
        private readonly IUnityContainer container;

        public ModuleRegistrar(IUnityContainer container)
        {
            this.container = container;
        }

        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            container.RegisterType<TFrom, TTo>();
        }
    }
}