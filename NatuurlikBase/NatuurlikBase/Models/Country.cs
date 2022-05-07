using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Country Name")]
        [MaxLength(25)]
        public string CountryName { get; set; }
    }
}
