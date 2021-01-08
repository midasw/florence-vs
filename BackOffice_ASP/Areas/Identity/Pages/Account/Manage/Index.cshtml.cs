using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BackOffice_ASP.Data;
using BackOffice_ASP.Helpers;
using BackOffice_ASP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackOffice_ASP.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public string Avatar { get; set; }
        public string AvatarUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public IFormFile Image { get; set; }
        }

        private void Load(ApplicationUser user)
        {
            Avatar = user.Avatar;
            AvatarUrl = user.AvatarUrl;

            Input = new InputModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool? delete = false)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            if (delete == true && !string.IsNullOrEmpty(user.Avatar))
            {
                var origFilePath = Path.Combine(uploads, user.Avatar);
                if (System.IO.File.Exists(origFilePath))
                {
                    System.IO.File.Delete(origFilePath);
                }

                user.Avatar = null;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to delete avatar.";
                    return RedirectToPage();
                }
            }
            else if (Input.Image != null && Input.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Input.Image.CopyToAsync(memoryStream);

                    try
                    {
                        var img = Image.FromStream(memoryStream);

                        const int width = 300;
                        const int height = 300;

                        if (img.Width > width || img.Height > height)
                        {
                            img = img.ResizeImage(width, height);
                        }

                        var uniqueFileName = $@"avatar-{Guid.NewGuid().ToString("N")}.jpg";
                        var filePath = Path.Combine(uploads, uniqueFileName);

                        img.Save(filePath, ImageFormat.Jpeg);

                        if (!string.IsNullOrEmpty(user.Avatar))
                        {
                            var origFilePath = Path.Combine(uploads, user.Avatar);
                            if (System.IO.File.Exists(origFilePath))
                            {
                                System.IO.File.Delete(origFilePath);
                            }
                        }

                        user.Avatar = uniqueFileName;

                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            StatusMessage = "Unexpected error when trying to update avatar.";
                            return RedirectToPage();
                        }
                    }
                    catch
                    {
                        StatusMessage = "Invalid file type";
                        return RedirectToPage();
                    }
                }

                StatusMessage = "Successfully updated avatar";
                await _signInManager.RefreshSignInAsync(user);
            }

            return RedirectToPage();
        }
    }
}
