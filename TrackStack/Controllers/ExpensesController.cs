using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackStack.Data;
using TrackStack.Models;

namespace TrackStack.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string? CurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId();
            if (userId == null) return Challenge();

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return View(expenses);
        }

        public IActionResult ShowSearchForm() => View();

        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            var userId = CurrentUserId();
            if (userId == null) return Challenge();

            var results = await _context.Expenses
                .Where(e => e.UserId == userId && e.Description.Contains(SearchPhrase))
                .ToListAsync();

            return View("Index", results);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var userId = CurrentUserId();
            if (id == null || userId == null) return NotFound();

            var expenses = await _context.Expenses
                .FirstOrDefaultAsync(m => m.ID == id && m.UserId == userId);
            if (expenses == null) return NotFound();

            return View(expenses);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Amount,Type,Description")] Expenses expenses)
        {
            var userId = CurrentUserId();
            if (userId == null) return Challenge();

            if (ModelState.IsValid)
            {
                expenses.UserId = userId;
                _context.Add(expenses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenses);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var userId = CurrentUserId();
            if (id == null || userId == null) return NotFound();

            var expenses = await _context.Expenses
                .FirstOrDefaultAsync(e => e.ID == id && e.UserId == userId);
            if (expenses == null) return NotFound();

            return View(expenses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Amount,Type,Description")] Expenses expenses)
        {
            var userId = CurrentUserId();
            if (userId == null) return Challenge();
            if (id != expenses.ID) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.Expenses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.ID == id && e.UserId == userId);
                if (existing == null) return NotFound();

                expenses.UserId = userId;
                _context.Update(expenses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenses);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var userId = CurrentUserId();
            if (id == null || userId == null) return NotFound();

            var expenses = await _context.Expenses
                .FirstOrDefaultAsync(m => m.ID == id && m.UserId == userId);
            if (expenses == null) return NotFound();

            return View(expenses);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = CurrentUserId();
            if (userId == null) return Challenge();

            var expenses = await _context.Expenses
                .FirstOrDefaultAsync(e => e.ID == id && e.UserId == userId);

            if (expenses != null)
            {
                _context.Expenses.Remove(expenses);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ExpensesExists(int id)
        {
            var userId = CurrentUserId();
            return userId != null && _context.Expenses.Any(e => e.ID == id && e.UserId == userId);
        }
    }
}