using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Data;
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
        private readonly DatabaseContext _db;

        public UserCartVM UserCartVM { get; set; }
        public int OrderSubTotal { get; set; }
        public UserCartController(IUnitOfWork unitOfWork, DatabaseContext db )
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public ActionResult GetCountries()
        {
            return Json(_unitOfWork.Country.GetAll().ToList());
        }

        public ActionResult GetProvince(int countryId)
        {
            return Json(_db.Province.Where(x => x.CountryId == countryId).Select(x => new
            {
                Text = x.ProvinceName,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }


        [HttpGet]
        public ActionResult GetCity(int provinceId)
        {
            return Json(_db.City.Where(x => x.ProvinceId == provinceId).Select(x => new
            {
                Text = x.CityName,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }

        [HttpGet]
        public ActionResult GetSuburb(int cityId)
        {
            return Json(_db.Suburb.Where(x => x.CityId == cityId).Select(x => new
            {
                Text = x.SuburbName,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }
        public IActionResult Index()
        {
            //get user claims.
            var claimsId = (ClaimsIdentity)User.Identity;
            var claim = claimsId.FindFirst(ClaimTypes.NameIdentifier);

            UserCartVM = new UserCartVM()
            {
                CartList = _unitOfWork.UserCart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product"),
                Order = new()
            };

            //Capture different prices for Resellers.

            if (User.IsInRole(SR.Role_Reseller))
            {
                foreach (var userCartItem in UserCartVM.CartList)
                {
                    userCartItem.CartItemPrice = GetCartItemPrices(userCartItem.Count, userCartItem.Product.ResellerPrice);

                    UserCartVM.Order.OrderTotal += (userCartItem.CartItemPrice * userCartItem.Count);
                }
            }
            else
            {
                foreach (var userCartItem in UserCartVM.CartList)
                {
                    userCartItem.CartItemPrice = GetCartItemPrices(userCartItem.Count, userCartItem.Product.CustomerPrice);

                    UserCartVM.Order.OrderTotal += (userCartItem.CartItemPrice * userCartItem.Count);
                }
            }


            return View(UserCartVM);
        }

        public IActionResult CartSummary()
        {


            //get user claims.
            var claimsId = (ClaimsIdentity)User.Identity;
            var claim = claimsId.FindFirst(ClaimTypes.NameIdentifier);

            UserCartVM = new UserCartVM()
            {
                CartList = _unitOfWork.UserCart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product"),
                Order = new()
            };

            UserCartVM.CountryList = _unitOfWork.Country.GetAll().Select(i => new SelectListItem
            {
                Text = i.CountryName,
                Value = i.Id.ToString()
            });

            UserCartVM.ProvinceList = _unitOfWork.Province.GetAll().Select(i => new SelectListItem
            {
                Text = i.ProvinceName,
                Value = i.Id.ToString()
            });
            UserCartVM.CityList = _unitOfWork.City.GetAll().Select(i => new SelectListItem
            {
                Text = i.CityName,
                Value = i.Id.ToString()
            });
            UserCartVM.SuburbList = _unitOfWork.Suburb.GetAll().Select(i => new SelectListItem
            {
                Text = i.SuburbName,
                Value = i.Id.ToString()
            });
            //Populate Cart VM with the user's stored details.
            UserCartVM.Order.ApplicationUser = _unitOfWork.User.GetFirstOrDefault(u => u.Id == claim.Value);
            //Populate Order Summary Details
            UserCartVM.Order.FirstName = UserCartVM.Order.ApplicationUser.FirstName;
            UserCartVM.Order.Surname = UserCartVM.Order.ApplicationUser.Surname;
            UserCartVM.Order.PhoneNumber = UserCartVM.Order.ApplicationUser.PhoneNumber;
            UserCartVM.Order.StreetAddress = UserCartVM.Order.ApplicationUser.StreetAddress;
            UserCartVM.Order.Country = UserCartVM.Order.ApplicationUser.CountryId;
            UserCartVM.Order.Province = UserCartVM.Order.ApplicationUser.ProvinceId;
            UserCartVM.Order.City = UserCartVM.Order.ApplicationUser.CityId;
            UserCartVM.Order.Suburb = UserCartVM.Order.ApplicationUser.SuburbId;


            if (User.IsInRole(SR.Role_Reseller))
            {
                foreach (var userCartItem in UserCartVM.CartList)
                {
                    userCartItem.CartItemPrice = GetCartItemPrices(userCartItem.Count, userCartItem.Product.ResellerPrice);

                    UserCartVM.Order.OrderTotal += (userCartItem.CartItemPrice * userCartItem.Count);
                }
            }
            else
            {
                foreach (var userCartItem in UserCartVM.CartList)
                {
                    userCartItem.CartItemPrice = GetCartItemPrices(userCartItem.Count, userCartItem.Product.CustomerPrice);

                    UserCartVM.Order.OrderTotal += (userCartItem.CartItemPrice * userCartItem.Count);
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
            if (cart.Count == 0)
            {
                _unitOfWork.UserCart.Remove(cart);
            }
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
