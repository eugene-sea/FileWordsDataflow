namespace FileWordsDataflow.Web
{
    using System;
    using Common;
    using Microsoft.Practices.Unity;

    internal static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            // TODO: ApplicationParameters should be created elsewhere
            ModuleLoader.LoadContainer(new ApplicationParameters(), container, ".\\bin", "FileWordsDataflow.*.dll");
        }
    }
}