using BankingSystem1.Models;
using BankingSystem1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TransactionController : Controller
{
    private readonly BankingContext _context;

    public TransactionController(BankingContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Auth");

        var userAccounts = _context.Accounts
            .Where(a => a.UserId == userId && a.AccountStatusId == 6)
            .Include(a => a.AccountType)
            .Include(a => a.Currency)
            .ToList();

        ViewBag.Accounts = userAccounts;

        return View();
    }

    [HttpPost]
    public IActionResult Create(int senderAccountId, string recipientPhone, int recipientAccountId, decimal amount)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Auth");

        var userAccounts = _context.Accounts
            .Where(a => a.UserId == userId && a.AccountStatusId == 6)
            .Include(a => a.AccountType)
            .Include(a => a.Currency)
            .ToList();
        ViewBag.Accounts = userAccounts;

        var sender = _context.Accounts
            .Include(a => a.Currency)
            .FirstOrDefault(a => a.AccountId == senderAccountId && a.UserId == userId);
        if (sender == null || sender.AccountStatusId != 6)
        {
            ViewBag.Error = "Invalid or inactive sender account.";
            return View();
        }

        var receiverUser = _context.Users.FirstOrDefault(u => u.PhoneNo == recipientPhone);
        if (receiverUser == null)
        {
            ViewBag.Error = "Recipient mobile number not found.";
            return View();
        }

        var receiver = _context.Accounts
            .Include(a => a.Currency)
            .FirstOrDefault(a => a.AccountId == recipientAccountId && a.UserId == receiverUser.UserId);
        if (receiver == null || receiver.AccountStatusId != 6)
        {
            ViewBag.Error = "Invalid or inactive recipient account.";
            return View();
        }

        if (amount <= 0)
        {
            ViewBag.Error = "Amount must be greater than zero.";
            return View();
        }

        if (amount > sender.Balance)
        {
            ViewBag.Error = "Insufficient balance.";
            return View();
        }

        // Currency Conversion
        var senderCurrency = _context.Currencies.FirstOrDefault(c => c.CurrencyId == sender.CurrencyId);
        var receiverCurrency = _context.Currencies.FirstOrDefault(c => c.CurrencyId == receiver.CurrencyId);

        if (senderCurrency == null || receiverCurrency == null)
        {
            ViewBag.Error = "Currency information missing.";
            return View();
        }

        decimal exchangeRate;
        decimal receiverAmount;

        if (senderCurrency.CurrencyId == receiverCurrency.CurrencyId)
        {
            exchangeRate = 1m;
            receiverAmount = amount;
        }
        else if (senderCurrency.Code == "EGP")
        {
            exchangeRate = 1 / receiverCurrency.ExchangeRateToEGP;
            receiverAmount = amount * exchangeRate;
        }
        else if (receiverCurrency.Code == "EGP")
        {
            exchangeRate = senderCurrency.ExchangeRateToEGP;
            receiverAmount = amount * exchangeRate;
        }
        else
        {
            decimal senderToEGP = senderCurrency.ExchangeRateToEGP;
            decimal receiverToEGP = receiverCurrency.ExchangeRateToEGP;
            exchangeRate = senderToEGP / receiverToEGP;
            receiverAmount = amount * exchangeRate;
        }


        sender.Balance -= amount;
        receiver.Balance += receiverAmount;

        var transaction = new Transaction
        {
            SenderAccountId = sender.AccountId,
            ReceiverAccountId = receiver.AccountId,
            SenderAmount = amount,
            ReceiverAmount = receiverAmount,
            ExchangeRate = exchangeRate,
            Timestamp = DateTime.Now
        };

        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        return RedirectToAction("MyTransactions");
    }

    [HttpGet]
    public IActionResult MyTransactions()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var sent = _context.Transactions
            .Include(t => t.SenderAccount).ThenInclude(a => a.Currency)
            .Include(t => t.SenderAccount).ThenInclude(a => a.User)
            .Include(t => t.ReceiverAccount).ThenInclude(a => a.Currency)
            .Include(t => t.ReceiverAccount).ThenInclude(a => a.User)
            .Where(t => t.SenderAccount.UserId == userId)
            .OrderByDescending(t => t.Timestamp)
            .ToList();

        var received = _context.Transactions
            .Include(t => t.SenderAccount).ThenInclude(a => a.Currency)
            .Include(t => t.SenderAccount).ThenInclude(a => a.User)
            .Include(t => t.ReceiverAccount).ThenInclude(a => a.Currency)
            .Include(t => t.ReceiverAccount).ThenInclude(a => a.User)
            .Where(t => t.ReceiverAccount.UserId == userId)
            .OrderByDescending(t => t.Timestamp)
            .ToList();


        ViewBag.SentTotal = sent.Sum(t => t.SenderAmount);
        ViewBag.ReceivedTotal = received.Sum(t => t.ReceiverAmount);

        var viewModel = new TransactionListViewModel
        {
            SentTransactions = sent,
            ReceivedTransactions = received
        };

        return View(viewModel);
    }




    [HttpGet]
    public JsonResult GetAccountsByPhone(string phone)
    {
        var accounts = _context.Accounts
            .Include(a => a.AccountType)
            .Include(a => a.Currency)
            .Include(a => a.User)
            .Where(a => a.User.PhoneNo == phone && a.AccountStatusId == 6)
            .Select(a => new
            {
                accountId = a.AccountId,
                accountTypeName = a.AccountType.TypeName,
                currency = new
                {
                    name = a.Currency.Name
                },
                fullName = a.User.FirstName + " " + a.User.LastName
            })
            .ToList();

        return Json(accounts);
    }
}