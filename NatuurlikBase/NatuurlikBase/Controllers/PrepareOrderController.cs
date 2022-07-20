using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Models.ViewModels;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;
using System.Security.Claims;

namespace NatuurlikBase.Controllers
{
    public class PrepareOrderController : Controller
    {
        private readonly DatabaseContext _db;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWork _unitOfWork;


        public PrepareOrderController(DatabaseContext db, IEmailSender emailSender, IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork)
        {
            _db = db;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;

        }

        public async Task<IActionResult> Index()
        {
            var packageOrderTransactions = _db.OrderProduct.Include(c => c.Order).Include(b => b.Product);
            return View(await packageOrderTransactions.ToListAsync());
        }

        public ActionResult GetOrders(int orderId)
        {
            return Json(_db.Order.Where(x => x.Id == orderId).Select(x => new
            {
                Text = x.Id,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }


        public ActionResult GetProducts(int prodId)
        {
            return Json(_db.Products.Where(x => x.Id == prodId).Select(x => new
            {
                Text = x.Name,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }


        public IActionResult Create()
        {
           
                ViewData["OrderId"] = new SelectList(_db.Order, "Id", "Id");
                ViewData["ProductId"] = new SelectList(_db.Products, "Id", "Name");
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, OrderId,ProductQuantity, ProductId, TransactionDate")] PackageOrderProduct packageOrder)
        {

            PackageOrderVM packageOrderVM = new PackageOrderVM()
            {
                PackageOrderProduct = new(),
                OrdersList = _unitOfWork.Order.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Id.ToString(),
                    Value = i.Id.ToString()
                }),

                ProductList = _unitOfWork.Product.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            //packageOrder.OrderId = packageOrderVM.PackageOrderProduct.OrderId;
            //packageOrder.ProductId = packageOrderVM.PackageOrderProduct.OrderId;
            //packageOrder.ProductQuantity = packageOrderVM.PackageOrderProduct.ProductQuantity;

            if (ModelState.IsValid)
            {
                var prod = _db.Products.Where(c => c.Id == packageOrder.ProductId).FirstOrDefault();
                var ords = _db.Order.Where(c => c.Id == packageOrder.OrderId).FirstOrDefault();

                if (prod.QuantityOnHand > packageOrder.ProductQuantity || prod.QuantityOnHand == packageOrder.ProductQuantity)

                {
                    prod.QuantityOnHand -= packageOrder.ProductQuantity;
                    _db.OrderProduct.Add(packageOrder);
                    ViewBag.Confirmation = "Are you sure you want to proceed with removal?";
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Product Packaged Successfully.";
                    

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Insufficient stock levels to package requested quantity.";
                }

            }
           
            return View(packageOrder);
        }


    }
}
