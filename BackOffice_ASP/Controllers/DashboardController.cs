using BackOffice_ASP.Data;
using BackOffice_ASP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BackOffice_ASP.Models;

namespace BackOffice_ASP.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var announcements = await _context.Announcements
                .Where(a => a.IsPublished == true)
                .Include(a => a.Author)
                .Include(a => a.Editor)
                .OrderByDescending(a => a.DateEdited)
                .ToListAsync();

            return View(new DashboardViewModel
            {
                Announcements = announcements
            });
        }
    }
}
