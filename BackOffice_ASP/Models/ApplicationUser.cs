using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackOffice_ASP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateJoined { get; set; } = DateTime.Now;

        [PersonalData]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [PersonalData]
        [DataType(DataType.MultilineText)]
        public string StreetAddress { get; set; }

        [PersonalData]
        public string CityName { get; set; }

        [PersonalData]
        public string StateCode { get; set; }

        [PersonalData]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [PersonalData]
        [Display(Name = "Country")]
        public Country Country { get; set; }

        public string Avatar { get; set; }

        public string AvatarUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Avatar))
                {
                    return "/uploads/" + Avatar;
                }

                return "/img/default-avatar-300x300.png";
            }
        }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
