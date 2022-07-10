using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class VAT
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "V.A.T Percentage is required")]
        [Display(Name = "V.A.T Percentage ")]

        public int VATPercentage { get; set; }

        [Required]
        [Display(Name = "VAT Factor")]
        public decimal VATFactor { get; set; }

        [Required]
        [Display(Name = "VAT Status")]
        public string VATStatus { get; set; }

        [Required]
        public DateTime CreatedDate = DateTime.Now;


    }
}
