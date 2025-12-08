using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using votegdgc.Data;
using votegdgc.Models;

namespace votegdgc.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", new { returnUrl })
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // GET: /Account/GoogleResponse (callback từ Google)
        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string? returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Authentication failed";
                return RedirectToAction("Index", "Home");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

            var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var avatarUrl = claims?.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

            if (string.IsNullOrEmpty(googleId) || string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Invalid Google data";
                return RedirectToAction("Index", "Home");
            }

            // Tìm hoặc tạo user
            var user = await _context.Users
                .Include(u => u.Votes)
                .FirstOrDefaultAsync(u => u.GoogleId == googleId);

            if (user == null)
            {
                user = new User
                {
                    GoogleId = googleId,
                    Email = email,
                    Name = name,
                    AvatarUrl = avatarUrl
                };
                _context.Users.Add(user);
            }
            else
            {
                // Update info nếu có thay đổi
                user.Name = name;
                user.AvatarUrl = avatarUrl;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Welcome, {user.Name}!";
            return Redirect(returnUrl ?? "/");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "Logged out successfully";
            return RedirectToAction("Index", "Home");
        }
    }
}