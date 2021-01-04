using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackOffice_ASP.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

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
        public Country Country { get; set; }
    }
}
