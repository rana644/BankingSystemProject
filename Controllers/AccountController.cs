using BankingSystem1.Models;
using BankingSystem1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly BankingContext _context;

    public AccountController(BankingContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Setup()
    {
        var model = new AccountSetupViewModel
        {
            AccountTypes = _context.AccountTypes.ToList(),
            Branches = _context.Branches.ToList(),
            Currencies = _context.Currencies.ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Setup(AccountSetupViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            Console.WriteLine("No UserId found in session.");
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState is invalid.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("Error: " + error.ErrorMessage);
            }

            model.AccountTypes = _context.AccountTypes.ToList();
            model.Branches = _context.Branches.ToList();
            model.Currencies = _context.Currencies.ToList();
            return View(model);
        }

        var alreadyExists = _context.Accounts.Any(a =>
            a.UserId == userId.Value &&
            a.AccountTypeId == model.SelectedAccountTypeId &&
            a.CurrencyId == model.SelectedCurrencyId
        );

        if (alreadyExists)
        {
            ViewBag.Error = "You already have an account with the selected type and currency.";
            model.AccountTypes = _context.AccountTypes.ToList();
            model.Branches = _context.Branches.ToList();
            model.Currencies = _context.Currencies.ToList();
            return View(model);
        }

        var account = new Account
        {
            UserId = userId.Value,
            AccountTypeId = model.SelectedAccountTypeId,
            Balance = 1000,
            AccountStatusId = 6,
            BranchId = model.SelectedBranchId,
            DateOpened = DateOnly.FromDateTime(DateTime.Now),
            DateClosed = null,
            CurrencyId = model.SelectedCurrencyId
        };

        _context.Accounts.Add(account);
        int affectedRows = _context.SaveChanges();
        Console.WriteLine($"Rows affected: {affectedRows}");

        // ✅ Restore session to avoid logout after account setup
        HttpContext.Session.SetInt32("UserId", userId.Value);

        var user = _context.Users.FirstOrDefault(u => u.UserId == userId.Value);
        if (user != null)
        {
            HttpContext.Session.SetString("UserName", user.FirstName);
            HttpContext.Session.SetString("IsAdmin", (user.IsAdmin ?? false) ? "true" : "false");
        }

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult GetUserAccountPairs()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return Unauthorized();

        var existingPairs = _context.Accounts
            .Where(a => a.UserId == userId)
            .Select(a => new {
                AccountTypeId = a.AccountTypeId,
                CurrencyId = a.CurrencyId
            }).ToList();

        return Json(existingPairs);
    }
}
