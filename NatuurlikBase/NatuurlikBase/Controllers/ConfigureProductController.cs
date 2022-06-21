using Microsoft.AspNetCore.Mvc;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class ConfigureProductController : Controller
    {
        private readonly DatabaseContext _db;
        public ConfigureProductController(DatabaseContext db)
        {
            _db = db;
        }

        public IActionResult Index(int Id)
        {
            return View();
        }

        public IActionResult ProductConfiguration()
        {
            return View();
        }
    }
}
