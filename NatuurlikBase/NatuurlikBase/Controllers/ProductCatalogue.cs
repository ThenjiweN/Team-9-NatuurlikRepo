using Microsoft.AspNetCore.Mvc;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;

namespace NatuurlikBase.Controllers
{
    public class ProductCatalogue : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductCatalogue(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //Get all products and return to the view
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,Brand");
            return View(productList);
        }


        public IActionResult Item(int productId)
        {
            Cart cartItem = new()
            {
                Count = 1,
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,Brand")
            };

            return View(cartItem);

        }

    }
}
