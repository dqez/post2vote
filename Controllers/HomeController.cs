using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using votegdgc.Data;
using votegdgc.Models;
using votegdgc.ViewModels;

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
            ViewBag.CurrentUser = await GetCurrentUserAsync();
            return View();
        }

        // GET: /Home/Vote
        public async Task<IActionResult> Vote()
        {
            var projects = await _context.Projects
                .Include(p => p.User)
                .Include(p => p.Votes)
                .OrderByDescending(p => p.Votes.Count)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();

            var currentUser = await GetCurrentUserAsync();
            
            var viewModel = new VotePageViewModel
            {
                Projects = projects,
                CurrentUser = currentUser
            };

            ViewBag.CurrentUser = currentUser;

            return View(viewModel);
        }

        // GET: /Home/Leaderboard
        public async Task<IActionResult> Leaderboard()
        {
            var projects = await _context.Projects
                .Include(p => p.User)
                .Include(p => p.Votes)
                .OrderByDescending(p => p.Votes.Count)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();

            var currentUser = await GetCurrentUserAsync();
            
            var viewModel = new LeaderboardViewModel
            {
                Projects = projects,
                CurrentUser = currentUser
            };

            ViewBag.CurrentUser = currentUser;

            return View(viewModel);
        }
    }
}