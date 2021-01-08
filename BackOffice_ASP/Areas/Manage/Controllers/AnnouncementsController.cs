using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackOffice_ASP.Data;
using BackOffice_ASP.Models;
using Microsoft.AspNetCore.Authorization;
using BackOffice_ASP.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BackOffice_ASP.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Super,Visibility,Fleco")]
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnnouncementsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // TempData["StatusMessage"]
        [TempData]
        public string StatusMessage { get; set; }

        // GET: manage/announcements
        public async Task<IActionResult> Index(string sorting, string q)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "";
            }

            var query = _context.Announcements
                .Include(a => a.Author)
                .Include(a => a.Editor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(a => a.Subject.Contains(q) || a.Body.Contains(q));
            }

            switch (sorting)
            {
                case "subject_asc":
                    query = query.OrderBy(a => a.Subject);
                    break;
                case "subject_desc":
                    query = query.OrderByDescending(a => a.Subject);
                    break;
                case "date_asc":
                    query = query.OrderBy(a => a.DateEdited);
                    break;
                case "date_desc":
                default:
                    query = query.OrderByDescending(a => a.DateEdited);
                    break;
            }

            ViewData["SortOrder"] = sorting;
            ViewData["Search"] = q;

            return View(await query.ToListAsync());
        }

        // POST: manage/announcements
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string button, List<int> checkedIds)
        {
            if (checkedIds.Any())
            {
                var announcements = _context.Announcements.Where(a => checkedIds.Contains(a.Id)).ToList();

                if (announcements.Any())
                {
                    string succesMessage = string.Empty;

                    switch (button)
                    {
                        case "publish":
                            announcements.ForEach(a => a.IsPublished = true);
                            succesMessage = "Successfully published {0} announcement(s)";
                            break;

                        case "unpublish":
                            announcements.ForEach(a => a.IsPublished = false);
                            succesMessage = "Successfully unpublished {0} announcement(s)";
                            break;

                        case "delete":
                            return await DeleteMultiple(checkedIds);
                    }

                    int nRows = await _context.SaveChangesAsync();

                    if (nRows > 0)
                    {
                        StatusMessage = string.Format(succesMessage, nRows);
                    }
                }
            }

            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        // GET: manage/announcements/details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: manage/announcements/create
        public IActionResult Create()
        {
            return View();
        }

        // POST: manage/announcements/create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Body")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.Author = await _userManager.GetUserAsync(User);
                announcement.ParseMarkdown();

                _context.Add(announcement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }

        // GET: manage/announcements/edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(new EditAnnouncementViewModel
            {
                Id = announcement.Id,
                Subject = announcement.Subject,
                Body = announcement.Body
            });
        }

        // POST: manage/announcements/edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Body")] EditAnnouncementViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);

            announcement.Subject = viewModel.Subject;
            announcement.Body = viewModel.Body;
            announcement.IsEdited = true;
            announcement.DateEdited = DateTime.Now;
            announcement.Editor = await _userManager.GetUserAsync(User);

            announcement.ParseMarkdown();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }

        // GET: manage/announcements/delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: manage/announcements/delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Publish(List<int> checkedIds)
        //{
        //    _context.Announcements
        //        .Where(a => checkedIds.Contains(a.Id))
        //        .ToList()
        //        .ForEach(a => a.Published = true);

        //    int nRows = await _context.SaveChangesAsync();
        //    if (nRows > 0)
        //    {
        //        TempData["Message"] = $"Successfully published {nRows} announcement(s)";
        //    }

        //    return Redirect(HttpContext.Request.Headers["Referer"]);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Unpublish(List<int> checkedIds)
        //{
        //    _context.Announcements
        //        .Where(a => checkedIds.Contains(a.Id))
        //        .ToList()
        //        .ForEach(a => a.Published = false);

        //    int nRows = await _context.SaveChangesAsync();
        //    if (nRows > 0)
        //    {
        //        TempData["Message"] = $"Successfully unpublished {nRows} announcement(s)";
        //    }

        //    return Redirect(HttpContext.Request.Headers["Referer"]);
        //}

        // POST: manage/announcements/deletemultiple
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMultiple(List<int> checkedIds, bool confirmed = false)
        {
            var announcements = _context.Announcements.Where(a => checkedIds.Contains(a.Id));

            if (confirmed)
            {
                _context.RemoveRange(announcements);

                int nRows = await _context.SaveChangesAsync();

                if (nRows > 0)
                {
                    StatusMessage = $"Successfully deleted {nRows} announcement(s)";
                }

                return RedirectToAction("Index");
            }

            return View("DeleteMultiple", await announcements.ToListAsync());
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(e => e.Id == id);
        }
    }
}
