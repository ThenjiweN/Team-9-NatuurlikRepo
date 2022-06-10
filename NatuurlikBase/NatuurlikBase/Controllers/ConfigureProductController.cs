using Microsoft.AspNetCore.Mvc;

namespace NatuurlikBase.Controllers
{
    public class ConfigureProductController : Controller
    {
        public IActionResult Index(int Id)
        {
            return View();
        }

        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View();
        }
    }
}
