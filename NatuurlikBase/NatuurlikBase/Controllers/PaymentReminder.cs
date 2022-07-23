using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Data;

namespace NatuurlikBase.Controllers
{
    public class PaymentReminder : Controller
    {

        private readonly DatabaseContext _db;

        public PaymentReminder(DatabaseContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var current = _db.PaymentReminder.Where(x => x.Active == "True").FirstOrDefault();
            ViewData["Reminder"] = new SelectList(_db.PaymentReminder, "Id", "Days");
            return View(current);
        }

        public IActionResult Update(int id)
        {
            if (ModelState.IsValid)
            {
                if (_db.PaymentReminder.Any(c => c.Active == "True" && c.Id == id))
                {
                    TempData["Delete"] = "The settlement time is already active";
                }
                else
                {
                    var old = _db.PaymentReminder.Where(x => x.Active == "True").FirstOrDefault();
                    if (old != null)
                    {
                        old.Active = "False";
                    }
                    var current = _db.PaymentReminder.Where(z => z.Id == id).FirstOrDefault();
                    if (current != null)
                    {
                        current.Active = "True";
                    }
                    TempData["success"] = "Reseller Credit Settlement Time Updated Successfully";
                    _db.SaveChanges();

                }
            }
            return RedirectToAction("Index");
        }
    }
}
