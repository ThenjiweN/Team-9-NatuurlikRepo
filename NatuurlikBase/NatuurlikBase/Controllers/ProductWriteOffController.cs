using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class ProductWriteOffController : Controller
    {
        private readonly DatabaseContext _context;


        public ProductWriteOffController(DatabaseContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.ProductWriteOff.Include(c => c.WriteOffReason).Include(b => b.product);
            return View(await databaseContext.ToListAsync());
        }



        public IActionResult WriteOff()
        {

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["WriteOffReasonId"] = new SelectList(_context.WriteOffReason, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WriteOff([Bind("Id,WriteOffDate,writeOffQuantity, ProductId,writeOffReasonId")] WriteOffProduct productWriteOff)
        {
            if (ModelState.IsValid)
            {

                var item = _context.Products.Where(c => c.Id == productWriteOff.ProductId).FirstOrDefault();

                if (item.QuantityOnHand > productWriteOff.writeOffQuantity || item.QuantityOnHand == productWriteOff.writeOffQuantity)

                {
                    item.QuantityOnHand -= productWriteOff.writeOffQuantity;
                   


                    _context.Add(productWriteOff);

                    TempData["success"] = "Product Written-Off Successfully";
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Quantity On Hand Is...";
                }




            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productWriteOff.ProductId);
            ViewData["WriteOffReasonId"] = new SelectList(_context.WriteOffReason, "Id", "Name", productWriteOff.writeOffReasonId);

            return View(productWriteOff);
        }


    }
}
