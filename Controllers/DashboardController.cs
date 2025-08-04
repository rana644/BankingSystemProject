using BankingSystem1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BankingContext _context;

        public DashboardController(BankingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var user = await _context.Users.FindAsync(userId);
            var accounts = await _context.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.AccountStatus)
                .Include(a => a.Currency)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            ViewBag.UserName = user?.FirstName;
            return View(accounts);
        }
    }
}
