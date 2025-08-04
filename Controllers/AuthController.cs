using BankingSystem1.Models;
using BankingSystem1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BankingSystem1.Controllers
{
    public class AuthController : Controller
    {
        private readonly BankingContext _context;

        public AuthController(BankingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View("Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                if (user.PasswordHash != HashPassword(password))
                {
                    ViewBag.Error = "Invalid email or password.";
                    return View("Login");
                }

                if (!(user.IsAdmin ?? false))
                {
                    bool hasAccount = await _context.Accounts.AnyAsync(a => a.UserId == user.UserId);

                    // ✅ Set session BEFORE any redirects to preserve it
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.FirstName);
                    HttpContext.Session.SetString("IsAdmin", (user.IsAdmin ?? false) ? "true" : "false");

                    if (!hasAccount)
                    {
                        // Account not yet created, redirect to setup
                        return RedirectToAction("Setup", "Account");
                    }

                    // ✅ If has account, continue to dashboard
                    return RedirectToAction("Index", "Dashboard");
                }


                // Set session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.FirstName);
                HttpContext.Session.SetString("IsAdmin", (user.IsAdmin ?? false) ? "true" : "false");

                if ((user.IsAdmin ?? false))
                    return RedirectToAction("PendingRequests", "AccountRequest");

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // Check if user exists in AccountCreationRequests (still pending)
                var request = await _context.AccountCreationRequests.FirstOrDefaultAsync(r => r.Email == email);
                if (request != null && request.PasswordHash == HashPassword(password))
                {
                    return RedirectToAction("PendingApproval", "Auth");
                }

                ViewBag.Error = "Invalid email or password.";
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult PendingApproval()
        {
            return View("PendingApproval");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.PhoneNo == model.PhoneNo);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or phone number.";
                return View(model);
            }

            user.PasswordHash = HashPassword(model.NewPassword);
            await _context.SaveChangesAsync();

            ViewBag.Success = "Password updated successfully. You can now log in.";
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Login", "Auth"); // Redirect to login page
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToUpper();
        }
    }
}
