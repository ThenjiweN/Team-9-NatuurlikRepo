﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;
using PhoneNumbers;
using System.Security.Claims;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NatuurlikBase.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly DatabaseContext _db;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork uow, DatabaseContext db,IWebHostEnvironment hostEnvironment, IEmailSender emailSender, IConfiguration configuration)
        {
            _uow = uow;
            _db = db;
            _hostEnvironment = hostEnvironment;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public ActionResult GetQueryReasons(int queryReasonId)
        {
            return Json(_uow.QueryReason.GetAll(x => x.Id == queryReasonId).Select(x => new
            {
                Text = x.Name,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }


        public IActionResult Detail(int? orderId)
        {
            //Load all order details
            OrderVM orderVM = new OrderVM()
            {
                Order = _uow.Order.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser,Country,Province,City,Suburb,Courier"),
                OrderLine = _uow.OrderLine.GetAll(ol => ol.OrderId == orderId, includeProperties: "Product")
            };

            return View(orderVM);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveResellerOrder()
        {
            //Retrieve Order details from the db
            var orderRetrieved = _uow.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
            //Update order status to approved state and save changes to db.
            _uow.Order.UpdateOrderStatus(OrderVM.Order.Id, SR.ProcessingOrder);
            _uow.Save();

            var user = _db.User.Where(z => z.Id == orderRetrieved.ApplicationUserId).FirstOrDefault();
            string email = user.Email;
            string name = user.FirstName;
            string number = orderRetrieved.Id.ToString();
            string date = orderRetrieved.CreatedDate.ToString("M");
            string status = orderRetrieved.OrderPaymentStatus.ToString();
            //var callbackUrl = Url.Page("/Order");

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var template = System.IO.File.ReadAllText(Path.Combine(wwwRootPath, @"emailTemp\appOrderTemp.html"));
            template = template.Replace("[NAME]", name).Replace("[STATUS]", status)
                .Replace("[ID]", number).Replace("[DATE]", date);
            string message = template;

            _emailSender.SendEmailAsync(
            email,
            "Order Approved",
            message);

            TempData["Success"] = "Reseller Order has been approved successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderVM.Order.Id });
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            //Retrieve Order details from the db
            var orderRetrieved = _uow.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
            //Update order status to approved state and save changes to db.
            _uow.Order.UpdateOrderStatus(OrderVM.Order.Id, SR.OrderCancelled);
            _uow.Save();

            var user = _db.User.Where(z => z.Id == orderRetrieved.ApplicationUserId).FirstOrDefault();
            string email = user.Email;
            string name = user.FirstName;
            string number = orderRetrieved.Id.ToString();
            string date = orderRetrieved.CreatedDate.ToString("M");
            string status = orderRetrieved.OrderPaymentStatus.ToString();
            //var callbackUrl = Url.Page("/Order");

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var template = System.IO.File.ReadAllText(Path.Combine(wwwRootPath, @"emailTemp\canResOrderTemp.html"));
            template = template.Replace("[NAME]", name).Replace("[STATUS]", status)
                .Replace("[ID]", number).Replace("[DATE]", date);
            string message = template;

            _emailSender.SendEmailAsync(
            email,
            "Order Cancelled",
            message);

            TempData["Success"] = "Order has been cancelled successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderVM.Order.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessOrder()
        {
            //Retrieve Order details from the db
            var orderRetrieved = _uow.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
            //Update order status to approved state and save changes to db.
            _uow.Order.UpdateOrderStatus(OrderVM.Order.Id, SR.ProcessingOrder);
            _uow.Save();
            TempData["Success"] = "Order status updated successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderVM.Order.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectResellerOrder()
        {
            //Retrieve Order details from the db
            var orderRetrieved = _uow.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
            //Update order status to approved state and save changes to db.
            _uow.Order.UpdateOrderStatus(OrderVM.Order.Id, SR.OrderRejected);
            _uow.Save();

            var user = _db.User.Where(z => z.Id == orderRetrieved.ApplicationUserId).FirstOrDefault();
            string email = user.Email;
            string name = user.FirstName;
            string number = orderRetrieved.Id.ToString();
            string date = orderRetrieved.CreatedDate.ToString("M");
            string status = orderRetrieved.OrderPaymentStatus.ToString();
            //var callbackUrl = Url.Page("/Order");

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var template = System.IO.File.ReadAllText(Path.Combine(wwwRootPath, @"emailTemp\rejOrderTemp.html"));
            template = template.Replace("[NAME]", name).Replace("[STATUS]", status)
                .Replace("[ID]", number).Replace("[DATE]", date);
            string message = template;

            _emailSender.SendEmailAsync(
            email,
            "Order Rejected",
            message);

            TempData["Success"] = "Reseller Order has been rejected successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderVM.Order.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CaptureResellerPayment()
        {
            //Retrieve Order details from the db
            var orderRetrieved = _uow.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
            //Update order status to approved state and save changes to db.
            _uow.Order.UpdateOrderPaymentStatus(OrderVM.Order.Id, SR.OrderPaymentApproved);
            _uow.Save();
            TempData["Success"] = "Reseller Order payment captured successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderVM.Order.Id });

        }

        public IActionResult DispatchParcel()
        {
            //Retrieve Order details from the db
            var orderRetrieved = _uow.Order.GetFirstOrDefault(u => u.Id == OrderVM.Order.Id);
            //Capture required details in order to ship/dispatch parcel.
            orderRetrieved.ParcelTrackingNumber = OrderVM.Order.ParcelTrackingNumber;
            orderRetrieved.DispatchedDate = DateTime.Now;
            orderRetrieved.OrderStatus = SR.OrderDispatched;
            _uow.Order.Update(orderRetrieved);
            _uow.Save();

            string accountId = _configuration["AccountId"];
            string authToken = _configuration["AuthToken"];
            TwilioClient.Init(accountId, authToken);
            var number = orderRetrieved.PhoneNumber;
            var name = orderRetrieved.FirstName;
            var order = orderRetrieved.Id;
            var track = orderRetrieved.ParcelTrackingNumber;
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var numberProto = phoneUtil.Parse(number, "ZA");
            var formattedPhone = phoneUtil.Format(numberProto, PhoneNumberFormat.E164);
            var to = formattedPhone;
            var companyNr = "+18305216564";

            var message = MessageResource.Create(
                to,
                from: companyNr,
                body: $"Hi " + name + " your Natuurlik order #" + order + " has been dispatched, your tracking number is " + track);

            TempData["Success"] = "Order status updated successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderVM.Order.Id });

        }


        public IActionResult Index(string status)
        {
            IEnumerable<Order> orders;

            if (User.IsInRole(SR.Role_Admin) || User.IsInRole(SR.Role_SA))
            {
                //Retrieve all orders for Administrator and Sales Assistant roles.
                 orders = _uow.Order.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                //get the orders associated only with the customer or reseller.
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orders = _uow.Order.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            }
           

            switch(status)
            {
                case "processing":
                    orders = orders.Where(u => u.OrderStatus == SR.ProcessingOrder);
                    break;

                case "pending":
                    orders = orders.Where(o => o.OrderStatus == SR.OrderPending);
                    break;

                case "dispatched":
                    orders = orders.Where(o => o.OrderStatus == SR.OrderDispatched);
                    break;

                case "cancelled":
                    orders = orders.Where(o => o.OrderStatus == SR.OrderCancelled);
                    break;

                case "refundpending":
                    orders = orders.Where(o => o.OrderStatus == SR.OrderRefundPending);
                    break;

                case "refunded":
                    orders = orders.Where(o => o.OrderStatus == SR.OrderRefunded);
                    break;

                default:
                    break;
            }
            return View(orders);
        }

       

    }
}
