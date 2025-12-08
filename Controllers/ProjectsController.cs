using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using votegdgc.Data;
using votegdgc.Models;

namespace votegdgc.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
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

        // GET: /Projects/Create
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Projects/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project model)
        {

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            // Xử lý ThumbnailLink tùy chọn: nếu null/empty thì gán chuỗi rỗng và bỏ qua lỗi validation
            if (string.IsNullOrEmpty(model.ThumbnailLink))
            {
                model.ThumbnailLink = string.Empty;
                ModelState.Remove("ThumbnailLink");
            }

            if (!ModelState.IsValid)
                return View(model);

            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            model.UserId = user.Id;
            model.CreatedAt = DateTime.UtcNow;

            _context.Projects.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Project created successfully!";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Projects/Details/5
        //[HttpGet]
        //public async Task<IActionResult> Details(int id)
        //{
        //    var project = await _context.Projects
        //        .Include(p => p.User)
        //        .Include(p => p.Votes)
        //            .ThenInclude(v => v.User)
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (project == null)
        //        return NotFound();

        //    // Pass current user to view for checking vote status
        //    var currentUser = await GetCurrentUserAsync();
        //    ViewBag.CurrentUser = currentUser;

        //    return View(project);
        //}

        // POST: /Projects/Vote
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(int projectId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            // Check remaining votes
            if (user.RemainingVotes <= 0)
            {
                TempData["Error"] = "You have used all 5 votes!";
                return RedirectToAction("Index", "Home");
            }

            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                return NotFound();

            // Can't vote for own project
            if (project.UserId == user.Id)
            {
                TempData["Error"] = "You cannot vote for your own project!";
                return RedirectToAction("Index", "Home");
            }

            // Check if already voted
            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == user.Id && v.ProjectId == projectId);

            if (existingVote != null)
            {
                TempData["Error"] = "You have already voted for this project!";
                return RedirectToAction("Index", "Home");
            }

            // Create vote
            var vote = new Vote
            {
                UserId = user.Id,
                ProjectId = projectId
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Voted successfully! {user.RemainingVotes - 1} votes remaining.";
            return RedirectToAction("Index", "Home");
        }

        // POST: /Projects/Unvote
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unvote(int projectId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var vote = await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == user.Id && v.ProjectId == projectId);

            if (vote == null)
            {
                TempData["Error"] = "Vote not found!";
                return RedirectToAction("Index", "Home");
            }

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Unvoted successfully! {user.RemainingVotes + 1} votes remaining.";
            return RedirectToAction("Index", "Home");
        }
    }
}