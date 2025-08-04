
using BankingSystem1.Models;
using BankingSystem1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BankingSystem1.Controllers
{
    public class AccountRequestController : Controller
    {
        private readonly BankingContext _context;

        public AccountRequestController(BankingContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult CreateRequest()
        {
            return View();
        }

        // POST: /AccountRequest/CreateRequest
        [HttpPost]
        public IActionResult CreateRequest(AccountCreationRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View(model);
            }

            var entity = new AccountCreationRequest
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNo = model.PhoneNo,
                Address = model.Address,
                PasswordHash = HashPassword(model.Password),
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            _context.AccountCreationRequests.Add(entity);
            _context.SaveChanges();

            TempData["Success"] = "Request submitted successfully. Please wait for admin approval.";
            return RedirectToAction("Login", "Auth");
        }
        [HttpGet]
        public IActionResult PendingRequests()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (string.IsNullOrEmpty(isAdmin) || isAdmin != "true")
            {
                return RedirectToAction("Login", "Auth");
            }

            var allRequests = _context.AccountCreationRequests.ToList();
            return View(allRequests);
        }
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var request = _context.AccountCreationRequests.Find(id);
            if (request == null || request.Status != "Pending")
                return NotFound();

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNo = request.PhoneNo,
                Address = request.Address,
                PasswordHash = request.PasswordHash,
                IsAdmin = false
            };

            _context.Users.Add(user);
            request.Status = "Approved";
            _context.SaveChanges();

            return RedirectToAction("PendingRequests");
        }
        [HttpPost]
        public IActionResult Reject(int id)
        {
            var request = _context.AccountCreationRequests.Find(id);
            if (request == null || request.Status != "Pending")
                return NotFound();

            request.Status = "Rejected";
            _context.SaveChanges();

            return RedirectToAction("PendingRequests");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToUpper();
        }
        [HttpPost]
        public IActionResult ExtractAndParseTransactions()
        {
            _context.Database.ExecuteSqlRaw("EXEC ExtractTransactionsToLog");
            _context.Database.ExecuteSqlRaw("EXEC ParseTransactionLog");

            TempData["Success"] = "Transactions extracted and parsed successfully.";
            return RedirectToAction("ViewParsedTransactions");
        }

        [HttpGet]
        public IActionResult ViewParsedTransactions()
        {
            var parsedData = _context.ParsedTransactionData.ToList();
            return View(parsedData);
        }

    }
}

