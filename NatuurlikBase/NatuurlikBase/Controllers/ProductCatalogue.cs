using Microsoft.AspNetCore.Mvc;

namespace NatuurlikBase.Controllers
{
    public class ProductCatalogue : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Item()
        {
            return View();
        }


    }
}
