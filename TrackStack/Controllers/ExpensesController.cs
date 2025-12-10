using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrackStack.Data;
using TrackStack.Models;

namespace TrackStack.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
            
        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            return View(await _context.Expenses
                .Where(e => e.UserEmail == userEmail)
                .ToListAsync());
        }

        // GET: Expenses/ShowSearchForm
        [Authorize]
        public IActionResult ShowSearchForm()
        {
            return View();
        }

        // GET: Expenses/ShowSearchResults
        [Authorize]
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            return View("Index", await _context.Expenses
                .Where(e => e.UserEmail == userEmail && e.Description.Contains(SearchPhrase))
                .ToListAsync());
        }

        // GET: Expenses/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            var expenses = await _context.Expenses
                .FirstOrDefaultAsync(m => m.ID == id && m.UserEmail == userEmail);
            
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // GET: Expenses/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Amount,Type,Description")] Expenses expenses)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                expenses.UserEmail = userEmail;
                _context.Add(expenses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenses);
        }

        // GET: Expenses/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            var expenses = await _context.Expenses.FindAsync(id);
            
            if (expenses == null || expenses.UserEmail != userEmail)
            {
                return NotFound();
            }
            
            return View(expenses);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Amount,Type,Description")] Expenses expenses)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            if (id != expenses.ID)
            {
                return NotFound();
            }

            var existingExpense = await _context.Expenses.FindAsync(id);
            
            if (existingExpense == null || existingExpense.UserEmail != userEmail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingExpense.Amount = expenses.Amount;
                    existingExpense.Type = expenses.Type;
                    existingExpense.Description = expenses.Description;
                    
                    _context.Update(existingExpense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpensesExists(id))
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
            return View(expenses);
        }

        // GET: Expenses/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            var expenses = await _context.Expenses
                .FirstOrDefaultAsync(m => m.ID == id && m.UserEmail == userEmail);
            
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            var expenses = await _context.Expenses.FindAsync(id);
            
            if (expenses != null && expenses.UserEmail == userEmail)
            {
                _context.Expenses.Remove(expenses);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpensesExists(int id)
        {
            return _context.Expenses.Any(e => e.ID == id);
        }
    }
}
