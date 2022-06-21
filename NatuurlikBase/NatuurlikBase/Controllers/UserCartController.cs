using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;
using System.Security.Claims;

namespace NatuurlikBase.Controllers
{
    [Authorize]
    public class UserCartController : Controller
    {
        //DI
        private readonly IUnitOfWork _unitOfWork;

        public UserCartVM UserCartVM { get; set; }
        public int OrderSubTotal { get; set; }
        public UserCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //get user claims.
            var claimsId = (ClaimsIdentity)User.Identity;
            var claim = claimsId.FindFirst(ClaimTypes.NameIdentifier);

            UserCartVM = new UserCartVM()
            {
                CartList = _unitOfWork.UserCart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product")
            };

            if(User.IsInRole(SR.Role_Reseller))
			{
                foreach (var userCartItem in UserCartVM.CartList)
                {
                    userCartItem.CartItemPrice = GetCartItemPrices(userCartItem.Count, userCartItem.Product.ResellerPrice);
    
                    this.UserCartVM.CartTotal += userCartItem.CartItemPrice * userCartItem.Count;
                }
            }
            else
			{
                foreach (var userCartItem in UserCartVM.CartList)
                {
                    userCartItem.CartItemPrice = GetCartItemPrices(userCartItem.Count, userCartItem.Product.CustomerPrice);

                    this.UserCartVM.CartTotal += userCartItem.CartItemPrice * userCartItem.Count;
                }
            }
           
            
            return View(UserCartVM);
        }

        public IActionResult Increment (int cartId)
		{
            var cart = _unitOfWork.UserCart.GetFirstOrDefault(x => x.Id == cartId);
            _unitOfWork.UserCart.increaseCount(cart, 1);
            //Save new count to DB
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
            
		}

        public IActionResult Decrement(int cartId)
        {
            var cart = _unitOfWork.UserCart.GetFirstOrDefault(x => x.Id == cartId);
            _unitOfWork.UserCart.decreaseCount(cart, 1);
            //Save new count to DB
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Remove(int cartId)
        {
            var userCart = _unitOfWork.UserCart.GetFirstOrDefault(x => x.Id == cartId);
            _unitOfWork.UserCart.Remove(userCart);
            //Save updated cart item quantities to DB
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }

        //get the prices of cart items to calculate cart total.
        private decimal GetCartItemPrices(decimal quantity, decimal price)
		{
            return price;
		}

     
    }
}
