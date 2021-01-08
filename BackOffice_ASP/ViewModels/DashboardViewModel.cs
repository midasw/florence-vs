using BackOffice_ASP.Models;
using System;
using System.Collections.Generic;

namespace BackOffice_ASP.ViewModels
{
    public class DashboardViewModel
    {
        public ICollection<Announcement> Announcements { get; set; }
    }
}
