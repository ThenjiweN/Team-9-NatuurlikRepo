using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class ReviewReason
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Review Reason Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

