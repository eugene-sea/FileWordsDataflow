[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FileWordsDataflow.Web.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(FileWordsDataflow.Web.UnityWebActivator), "Shutdown")]

namespace FileWordsDataflow.Web
{
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity.Mvc;

    internal static class UnityWebActivator
    {
        public static void Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}