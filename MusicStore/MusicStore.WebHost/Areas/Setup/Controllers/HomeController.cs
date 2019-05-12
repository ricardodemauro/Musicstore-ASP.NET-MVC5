using Microsoft.AspNetCore.Mvc;

namespace MusicStore.WebHost.Areas.Setup.Controllers
{
    [Area("Setup")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
