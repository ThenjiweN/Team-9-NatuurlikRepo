using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class PaymentReminder
    {
        public int Id { get; set; }

        [Display(Name = "Days")]
        public string Days { get; set; }

        [Display(Name = "Value")]
        public int Value { get; set; }

        [Display(Name = "Active")]
        public string Active { get; set; }

    }
}
