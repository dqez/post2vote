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

        // GET: /Projects/MyProjects
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyProjects()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var projects = await _context.Projects
                .Include(p => p.Votes)
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(projects);
        }

        // GET: /Projects/Edit/5
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            // Only owner can edit
            if (project.UserId != user.Id)
                return Forbid();

            return View(project);
        }

        // POST: /Projects/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project model)
        {
            if (id != model.Id)
                return NotFound();

            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            // Only owner can edit
            if (project.UserId != user.Id)
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            project.Title = model.Title;
            project.Description = model.Description;
            project.Prompt = model.Prompt;
            project.ProjectLink = model.ProjectLink;
            project.ThumbnailLink = model.ThumbnailLink;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Project updated successfully!";
            return RedirectToAction("Details", new { id = project.Id });
        }

        // POST: /Projects/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            // Only owner can delete
            if (project.UserId != user.Id)
                return Forbid();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Project deleted successfully!";
            return RedirectToAction("MyProjects");
        }
    }
}