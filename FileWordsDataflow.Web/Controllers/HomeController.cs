namespace FileWordsDataflow.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Services;

    public class HomeController : Controller
    {
        private readonly IFileWordsService fileWordsService;

        public HomeController(IFileWordsService fileWordsService)
        {
            this.fileWordsService = fileWordsService;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "It is OK")]
        public async Task<ActionResult> Index(int skip = 0, int take = 20)
        {
            ViewBag.Skip = skip;
            ViewBag.Take = take;
            ViewBag.FileWords = await fileWordsService.GetFileWordStatsAsync(skip, take);
            return View();
        }

        public async Task<ActionResult> RefreshData(string folderPath, string searchPattern)
        {
            await fileWordsService.ParseFilesAsync(folderPath, searchPattern);
            return RedirectToAction("Index");
        }
    }
}