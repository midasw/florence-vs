using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice_ASP.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
