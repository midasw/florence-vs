using BackOffice_ASP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice_ASP.Areas.Manage.ViewModels
{
    public class CreateUserViewModel
    {
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public SelectList Roles { get; set; }
    }
}
