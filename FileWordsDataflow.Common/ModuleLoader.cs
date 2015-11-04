namespace FileWordsDataflow.Common
{
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using Microsoft.Practices.Unity;

    public static class ModuleLoader
    {
        public static void LoadContainer(
            ApplicationParameters parameters, IUnityContainer container, string path, string pattern)
        {
            var importDef = BuildImportDefinition();
            using (var dirCat = new DirectoryCatalog(path, pattern))
            using (var aggregateCatalog = new AggregateCatalog())
            {
                aggregateCatalog.Catalogs.Add(dirCat);
                using (var componsitionContainer = new CompositionContainer(aggregateCatalog))
                {
                    var modules = componsitionContainer.GetExports(importDef).Select(e => e.Value).OfType<IModule>();
                    var registrar = new ModuleRegistrar(container);
                    foreach (var module in modules)
                    {
                        module.Initialize(parameters, registrar);
                    }
                }
            }
        }

        private static ImportDefinition BuildImportDefinition()
        {
            return new ImportDefinition(
                def => true, typeof(IModule).FullName, ImportCardinality.ZeroOrMore, false, false);
        }
    }
}