using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class QueryReason
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Query Reason Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}