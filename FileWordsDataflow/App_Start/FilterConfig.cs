namespace FileWordsDataflow
{
    using System.Web.Mvc;

    internal static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}