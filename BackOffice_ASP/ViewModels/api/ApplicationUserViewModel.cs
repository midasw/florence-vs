using BackOffice_ASP.Models;
using System;
using System.Collections.Generic;

namespace BackOffice_ASP.ViewModels.api
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public DateTime DateJoined { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Country Country { get; set; }
        public string Avatar { get; set; }
        public string AvatarUrl { get; set; }
        public List<ApplicationRoleViewModel> Roles { get; set; }

        public ApplicationUserViewModel()
        {
            Roles = new List<ApplicationRoleViewModel>();
        }

        public ApplicationUserViewModel(ApplicationUser user) : this()
        {
            Id = user.Id;
            DateJoined = user.DateJoined;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Country = user.Country;
            Avatar = user.Avatar;
            AvatarUrl = user.AvatarUrl;

            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    if (userRole.Role != null)
                    {
                        Roles.Add(new ApplicationRoleViewModel
                        {
                            Name = userRole.Role.Name,
                            Description = userRole.Role.Description,
                        });
                    }
                }
            }
        }
    }
}