using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;
using Stripe.Checkout;
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
        public UserCartController(IUnitOfWork unitOfWork, DatabaseContext db)
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

        [HttpGet]
        public ActionResult GetCouriers(int courierId)
        {
            return Json(_db.Courier.Where(x => x.Id == courierId).Select(x => new
            {
                Text = x.CourierName,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }

        [HttpPost]
        public IActionResult GetCourierFee(int courierId)
        {
            return Json(_db.Courier.Where(x => x.Id == courierId).Select(x => x.CourierFee).FirstOrDefault());
                
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
            UserCartVM.CourierList = _unitOfWork.Courier.GetAll().Select(i => new SelectListItem
            {
                Text = i.CourierName,
                Value = i.Id.ToString()
            });

            //var deliveryMethods = _db.Courier.ToList();

            //UserCartVM.Couriers = deliveryMethods;

            var courier = _db.Courier.FirstOrDefault();

            var currentVAT = _db.VAT.FirstOrDefault(x => x.VATStatus == "Active");

           //Get Current VAT and calculate inclusive VAT amount for Order.
           var activeVAT = _db.VAT.FirstOrDefault(x => x.VATStatus == "Active");
           

            var vatFactor = Convert.ToDecimal(activeVAT?.VATPercentage / 100.00);

            var calculatedVAT = UserCartVM.Order.OrderTotal * vatFactor;

            var vat = _db.VAT.FirstOrDefault(x => x.VATStatus == "Active");


            
            //Populate Cart VM with the user's stored details.
            UserCartVM.Order.ApplicationUser = _unitOfWork.User.GetFirstOrDefault(u => u.Id == claim.Value);
            //Populate Order Summary Details
            UserCartVM.Order.FirstName = UserCartVM.Order.ApplicationUser.FirstName;
            UserCartVM.Order.Surname = UserCartVM.Order.ApplicationUser.Surname;
            UserCartVM.Order.PhoneNumber = UserCartVM.Order.ApplicationUser.PhoneNumber;
            UserCartVM.Order.StreetAddress = UserCartVM.Order.ApplicationUser.StreetAddress;
            UserCartVM.Order.CountryId = UserCartVM.Order.ApplicationUser.CountryId;
            UserCartVM.Order.ProvinceId = UserCartVM.Order.ApplicationUser.ProvinceId;
            UserCartVM.Order.CityId = UserCartVM.Order.ApplicationUser.CityId;
            UserCartVM.Order.SuburbId = UserCartVM.Order.ApplicationUser.SuburbId;
            UserCartVM.Order.VAT = vat;
            if(vat == null)
            {

            }
            else
            {
                UserCartVM.Order.VATId = vat.Id;
            }
            
            UserCartVM.Order.InclusiveVAT = calculatedVAT;

          
            



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

        //Submit Order Details for validation and redirect to payment for customer or create order for Reseller

        [HttpPost]
        [ActionName("CartSummary")]
        [ValidateAntiForgeryToken]
        public IActionResult CartSummaryPOST(UserCartVM UserCartVM)
        {
            //get user claims.
            var claimsId = (ClaimsIdentity)User.Identity;
            var claim = claimsId.FindFirst(ClaimTypes.NameIdentifier);




            UserCartVM.CartList = _unitOfWork.UserCart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product");
            UserCartVM.Order.OrderPaymentStatus = SR.ResellerDelayedPayment;
            UserCartVM.Order.OrderStatus = SR.OrderPending;
            UserCartVM.Order.CreatedDate = System.DateTime.Now;
            UserCartVM.Order.ApplicationUserId = claim.Value;
            //Add Courier fees
            UserCartVM.Order.CourierId = UserCartVM.Order.CourierId;
            var deliveryFee = _db.Courier.FirstOrDefault(x => x.Id == UserCartVM.Order.CourierId);
            if (deliveryFee != null)
            {
                UserCartVM.Order.DeliveryFee = deliveryFee.CourierFee;
            }





            //Calculate Order Total
            if (User.IsInRole(SR.Role_Reseller))
            {

                UserCartVM.Order.OrderPaymentStatus = SR.ResellerDelayedPayment;
                UserCartVM.Order.OrderStatus = SR.OrderPending;
                foreach (var userCartItem in UserCartVM.CartList)
                {
                    userCartItem.CartItemPrice = GetCartItemPrices(userCartItem.Count, userCartItem.Product.ResellerPrice);

                    UserCartVM.Order.OrderTotal += (userCartItem.CartItemPrice * userCartItem.Count);
                    UserCartVM.Order.OrderTotal += UserCartVM.Order.DeliveryFee;
                    UserCartVM.Order.IsResellerOrder = true;
                }
            }
            //Display Customer prices where user role not "Reseller"
            else
            {
                foreach (var userCartItem in UserCartVM.CartList)
                {
                    UserCartVM.Order.OrderPaymentStatus = SR.OrderPaymentApproved;
                    UserCartVM.Order.OrderStatus = SR.ProcessingOrder;
                    userCartItem.CartItemPrice = GetCartItemPrices(userCartItem.Count, userCartItem.Product.CustomerPrice);
                    UserCartVM.Order.OrderTotal += (userCartItem.CartItemPrice * userCartItem.Count);
                    UserCartVM.Order.OrderTotal += UserCartVM.Order.DeliveryFee;
                    UserCartVM.Order.IsResellerOrder = false;
                }
            }
            //Get the Current VAT % from db
            var activeVAT = _db.VAT.Where(x => x.VATStatus == "Active").FirstOrDefault();
            var calculatedVAT=0.00M;
            //VAT + 100 numerator value for calculation
            if (activeVAT != null)
            {
                var vatFactor = Convert.ToDecimal(activeVAT.VATPercentage / 100.00);

                 calculatedVAT = UserCartVM.Order.OrderTotal * vatFactor;
            }
            else
            {
                calculatedVAT = 0.00M;
            }
            
            //calculate inclusive VAT
            UserCartVM.Order.InclusiveVAT = calculatedVAT;


            //Add Captured order details
            _unitOfWork.Order.Add(UserCartVM.Order);

            //Save Changes to the Database
            _unitOfWork.Save();

            //Capture details for each product included in order.
            foreach (var userCartItem in UserCartVM.CartList)
            {
                OrderLine orderLine = new()
                {
                    ProductId = userCartItem.ProductId,
                    OrderId = UserCartVM.Order.Id,
                    Price = userCartItem.CartItemPrice,
                    Count = userCartItem.Count
                };
                _unitOfWork.OrderLine.Add(orderLine);
                _unitOfWork.Save();
            }

            //Is the Order being placed by a Reseller? Let's check.

            if (User.IsInRole(SR.Role_Reseller))
            {
                //Redirect to the Confirmation page.
                return RedirectToAction("ResellerOrderConfirmation", "UserCart", new { id = UserCartVM.Order.Id });
            }

            else
            {
                //Temporary Stripe Payment Processing
                var domain = "https://localhost:7056/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card",
                    },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"usercart/OrderConfirmation?id={UserCartVM.Order.Id}",
                    CancelUrl = domain + $"usercart/index",
                };

                foreach (var item in UserCartVM.CartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.CartItemPrice * 100),
                            Currency = "zar",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            },
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.Order.UpdateStripePayment(UserCartVM.Order.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);

                //If the payment has been made successfully, clear the shopping cart details from database for specific user.
            }
        }



        public IActionResult OrderConfirmation(int id)
        {
            Order order = _unitOfWork.Order.GetFirstOrDefault(x => x.Id == id);

            //Need to validate payment.
            if (order.OrderPaymentStatus != SR.ResellerDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(order.SessionId);

                //Perform check to see if payment was made successfully.

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.Order.UpdateOrderStatus(id, SR.ProcessingOrder, SR.OrderPaymentApproved);
                    _unitOfWork.Save();
                }
            }

            //Clear shopping cart details from DB.
            List<Cart> userCarts = _unitOfWork.UserCart.GetAll(uc => uc.ApplicationUserId == order.ApplicationUserId).ToList();
            _unitOfWork.UserCart.RemoveRange(userCarts);
            _unitOfWork.Save();
            return View(id);

        }

        //Create Reseller Order and display Confirmation Page Details
        public IActionResult ResellerOrderConfirmation(int id)
        {
            Order order = _unitOfWork.Order.GetFirstOrDefault(x => x.Id == id);
            List<Cart> userCarts = _unitOfWork.UserCart.GetAll(uc => uc.ApplicationUserId == order.ApplicationUserId).ToList();
            _unitOfWork.UserCart.RemoveRange(userCarts);
            _unitOfWork.Save();
            return View(id);

        }


        public IActionResult Increment(int cartId)
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
