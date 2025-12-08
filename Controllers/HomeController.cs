using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using votegdgc.Data;
using votegdgc.Models;

namespace votegdgc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Helper method: Lấy current user
        private async Task<User?> GetCurrentUserAsync()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return null;

            var googleId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(googleId))
                return null;

            return await _context.Users
                .Include(u => u.Votes)
                .FirstOrDefaultAsync(u => u.GoogleId == googleId);
        }

        // GET: /
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.User)
                .Include(p => p.Votes)
                .OrderByDescending(p => p.Votes.Count)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.CurrentUser = await GetCurrentUserAsync();

            return View(projects);
        }
    }
}