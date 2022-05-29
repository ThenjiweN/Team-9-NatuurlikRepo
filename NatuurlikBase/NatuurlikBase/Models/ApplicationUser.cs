using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NatuurlikBase.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
 
    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[ a-zA-Z ]+$", ErrorMessage = "Invalid First Name provided. No digits or special characters are allowed.")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[ a-zA-Z ]+$", ErrorMessage = "Invalid Surname provided. No digits or special characters are allowed.")]
    public string Surname { get; set; }

    [Required]
    [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter a valid Phone Number.")]
    [MaxLength(10)]
    [Display(Name ="Phone Number")]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "Street Address")]
    public string StreetAddress { get; set; }
    public int? CountryId { get; set; }
    [ForeignKey("CountryId")]
    [ValidateNever]
    public Country Country { get; set; }


    public int? ProvinceId { get; set; }
    [ForeignKey("ProvinceId")]
    [ValidateNever]
    public Province Province { get; set; }


    public int? CityId { get; set; }
    [ForeignKey("CityId")]
    [ValidateNever]
    public City City { get; set; }

    public int? SuburbId { get; set; }
    [ForeignKey("SuburbId")]
    [ValidateNever]
    public Suburb Suburb { get; set; }

}

