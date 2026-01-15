using Microsoft.AspNetCore.Mvc;

namespace CCTVSite.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
