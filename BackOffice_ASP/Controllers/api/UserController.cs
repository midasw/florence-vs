using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackOffice_ASP.Data;
using BackOffice_ASP.Models;
using BackOffice_ASP.ViewModels.api;
using BackOffice_ASP.Entities;
using Microsoft.AspNetCore.Identity;
using BackOffice_ASP.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BackOffice_ASP.Controllers.api
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;

        public UserController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<AppSettings> appSettings
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }


        // GET: api/Users
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUserViewModel>>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.Country)
                .Select(u => new ApplicationUserViewModel(u))
                .ToListAsync();
        }

        // GET: api/Users/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserViewModel>> GetUser(string id)
        {
            var user = await _context.Users
                .Include(u => u.Country)
                .Include(u => u.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .Where(u => u.Id == id)
                .Select(u => new ApplicationUserViewModel(u))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("authenticate")]
        public async Task<object> Authenticate([FromBody] ApiUser apiUser)
        {
            var applicationUser = await _userManager.FindByEmailAsync(apiUser.Email);

            if (applicationUser != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(
                applicationUser.UserName, apiUser.Password, false, false);

                if (signInResult.Succeeded)
                {
                    apiUser.Token = _appSettings.GenerateJwtToken(applicationUser).ToString();
                    return apiUser;
                }
            }

            return BadRequest(new { message = "Email or password is incorrect" });
        }
    }
}
