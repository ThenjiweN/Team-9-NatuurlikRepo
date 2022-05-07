
using System.ComponentModel.DataAnnotations;
namespace NatuurlikBase.Models
{
    public class ReturnReason
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Return Reason")]
        [MaxLength(50)]
        public string ReturnReasonName { get; set; }
    }
}

