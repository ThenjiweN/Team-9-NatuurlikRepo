using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class WriteOffReason
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Write-Off Reason")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
