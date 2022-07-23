using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class Order
    {
        //Add properties for Order. 
        //Model class to be associated with Order Lines.
        //This model represents the Order Header component.

        public int Id { get; set; }

        //Link order to a specific user registered on ByteXecom
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        //Control order states via a [Status] property.
        //Values supplied via enum class.
        public string? OrderStatus { get; set; }

        //Resellers have the option to pay within 30 day of placing the order. Used to track payment status.
        public string? OrderPaymentStatus { get; set; }

        public decimal OrderTotal { get; set; }

        [Required]
        [Display(Name ="Courier")]
        public int? CourierId { get; set; }

        [ValidateNever]
        public Courier Courier { get; set; }

        [Required]
        public decimal DeliveryFee { get; set; }

        public string? ParcelTrackingNumber { get; set; }


        [Required]
        public DateTime CreatedDate { get; set; }

        //Not required at creation of order.
        //Date and Time when the parcel has been sent out for delivery.

        public DateTime PaymentDueDate { get; set; }
        public DateTime DispatchedDate { get; set; }

        //Transaction via payment processing gateway - Stripe as POC until deployed and can fully integrate with Payfast.
        public string? SessionId { get; set; }

        public string? PaymentIntentId { get; set; }


        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public int? CountryId { get; set; }
        [ValidateNever]
        public Country Country { get; set; }

        [Required]
        public int? ProvinceId { get; set; }
        [ValidateNever]
        public Province Province { get; set; }

        [Required]

        public int? SuburbId { get; set; }

        [ValidateNever]
        public Suburb Suburb { get; set; }
        [Required]
        public int? CityId { get; set; }
        [ValidateNever]
        public City City { get; set; }

        public int? VATId { get; set; }
        [ValidateNever]
        public VAT VAT { get; set; }

        [Required]
        public decimal InclusiveVAT { get; set; }

        public bool? IsResellerOrder { get; set; }

        public int? PaymentReminderId { get; set; }
        [ValidateNever]
        public PaymentReminder PaymentReminder { get; set; }

    }
}
