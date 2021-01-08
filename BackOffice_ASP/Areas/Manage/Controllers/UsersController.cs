using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BackOffice_ASP.Data;
using BackOffice_ASP.Models;
using Microsoft.EntityFrameworkCore;
using BackOffice_ASP.Areas.Manage.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackOffice_ASP.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Super,Visibility,Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // TempData["StatusMessage"]
        [TempData]
        public string StatusMessage { get; set; }

        // GET: manage/users
        public async Task<IActionResult> Index(string sorting, string q)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "";
            }

            var query = _context.Users
                .Include(u => u.Country)
                .Include(u => u.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(a => a.FirstName.Contains(q) || a.LastName.Contains(q));
            }

            switch (sorting)
            {
                case "firstname_asc":
                    query = query.OrderBy(a => a.FirstName);
                    break;
                case "firstname_desc":
                    query = query.OrderByDescending(a => a.FirstName);
                    break;
                case "lastname_asc":
                    query = query.OrderBy(a => a.LastName);
                    break;
                case "lastname_desc":
                    query = query.OrderByDescending(a => a.LastName);
                    break;
                case "country_asc":
                    query = query.OrderBy(a => a.Country.Name);
                    break;
                case "country_desc":
                    query = query.OrderByDescending(a => a.Country.Name);
                    break;
                case "date_asc":
                    query = query.OrderBy(a => a.DateJoined);
                    break;
                case "date_desc":
                default:
                    query = query.OrderByDescending(a => a.DateJoined);
                    break;
            }

            ViewData["SortOrder"] = sorting;
            ViewData["Search"] = q;

            return View(await query.ToListAsync());
        }

        // GET: manage/users/create
        public IActionResult Create()
        {
            var roles = _context.Roles.OrderBy(c => c.Description).ToList();

            var viewModel = new CreateUserViewModel
            {
                Roles = new SelectList(roles, "Name", "Description")
            };

            return View(viewModel);
        }

        // POST: manage/users/create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                };

                var result = await _userManager.CreateAsync(applicationUser);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, viewModel.RoleName);

                    StatusMessage = "User has been created successfully";
                }

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
    }
}
