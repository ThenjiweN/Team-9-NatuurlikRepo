using Microsoft.AspNetCore.Mvc;

namespace NatuurlikBase.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
