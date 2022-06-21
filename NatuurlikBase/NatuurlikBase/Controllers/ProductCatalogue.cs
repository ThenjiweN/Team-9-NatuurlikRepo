using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;
using System.Security.Claims;

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
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,Brand")
            };

            return View(cartItem);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Item(Cart userCart)
        {
            //Get application user
            var claimsId = (ClaimsIdentity)User.Identity;
            var claim = claimsId.FindFirst(ClaimTypes.NameIdentifier);
            userCart.ApplicationUserId = claim.Value;

            Cart cart = _unitOfWork.UserCart.GetFirstOrDefault(u=> u.ProductId == userCart.ProductId && u.ApplicationUserId == claim.Value );

            //check for existing cart
            if(cart == null)
            {
                _unitOfWork.UserCart.Add(userCart);
            }
            else
            {
                _unitOfWork.UserCart.increaseCount(cart, userCart.Count);
            }
            //save cart changes to database
            _unitOfWork.Save();
            //Redirect to Product Catalogue Index page if saved successfully.
            return RedirectToAction("Index");

        }

    }
}
