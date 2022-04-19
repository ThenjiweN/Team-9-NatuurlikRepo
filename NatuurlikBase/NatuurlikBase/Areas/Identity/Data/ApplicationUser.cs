using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NatuurlikBase.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{

    [PersonalData]
    [Phone]
    [MaxLength(50)]
    [EmailAddress]
    [Range(1, 5000)]
    [Required]
    
    public string FirstName { get; set; }

    [PersonalData]
    public string LastName { get; set; }

    [PersonalData]
    public string PhoneNumber { get; set; }

}

