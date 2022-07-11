using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class InventoryProcuredController : Controller
    {
        private readonly DatabaseContext db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public InventoryProcuredController(DatabaseContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var databaseContext = db.InventoryProcured.Include(c => c.Supplier).Include(x => x.InventoryItem);
            return View(databaseContext.ToList());
        }

        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(db.Suppliers, "Id", "CompanyName");
            ViewData["ItemID"] = new SelectList(db.InventoryItem, "Id", "InventoryItemName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,SupplierId,InvoiceNo,ItemID,QuantityReceived,DateLogged")] InventoryProcured inventoryProcured)
        {
            if (ModelState.IsValid)

            {
                var inv = db.InventoryItem.Where(s => s.Id == inventoryProcured.ItemID).FirstOrDefault();
                if (inv != null)
                {
                    inv.QuantityOnHand += inventoryProcured.QuantityReceived;
                }

                db.InventoryProcured.Add(inventoryProcured);
                ViewBag.InventoryProcuredConfrimation = "Confirm Procured Inventory Details";
                db.SaveChanges();

                TempData["success"] = "Procured Inventory Captured Successfully!";
                TempData["NextCreation"] = "Hello World.";

                return RedirectToAction("Index");
            }
            ViewData["SupplierId"] = new SelectList(db.Suppliers, "Id", "CompanyName", inventoryProcured.SupplierId);
            ViewData["ItemID"] = new SelectList(db.InventoryItem, "Id", "InventoryItemName", inventoryProcured.ItemID);
            return View(inventoryProcured);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryProcured = await db.InventoryProcured.Include(c => c.Supplier).
                Include(x => x.InventoryItem).FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryProcured == null)
            {
                return NotFound();
            }

            return View(inventoryProcured);
        }


    }
}

