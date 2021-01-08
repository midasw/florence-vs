using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackOffice_ASP.Data;
using BackOffice_ASP.Models;

namespace BackOffice_ASP.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: announcements
        public async Task<IActionResult> Index()
        {
            var announcements = await _context.Announcements
                .Include(a => a.Author)
                .Include(a => a.Editor)
                .OrderByDescending(a => a.DateEdited)
                .ToListAsync();

            return View(announcements);
        }

        // GET: announcements/view/5
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .Include(a => a.Author)
                .Include(a => a.Editor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }
    }
}
