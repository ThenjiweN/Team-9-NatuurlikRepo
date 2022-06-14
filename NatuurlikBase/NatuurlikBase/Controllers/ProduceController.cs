using Microsoft.AspNetCore.Mvc;

namespace NatuurlikBase.Controllers
{
    public class ProduceController : Controller
    {
        public IActionResult Index(int Id)
        {
            return View();
        }

    }
}
