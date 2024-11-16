using BankingProject.Data;
using Microsoft.AspNetCore.Mvc;
using BankingProject.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BankingProject.DTO;
using System.Security.Principal;
using BankingProject.Models.Payment;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace BankingProject.Controllers

{
	public class LoginController : Controller
	{
		private readonly ApplicationDbContext _context;
		public LoginController(ApplicationDbContext context)
		{
			_context = context;
		}

		private readonly string email = "eli@gmail.com";
        private readonly string password = "1234";

        public IActionResult Index()
		{
			return View("LogIn");
		}

        // GET: Login/UserLogin
        [HttpGet]
        public IActionResult UserLogin()
		{
			return View();
		}
        // GET: Login/AdminLogin
        public IActionResult AdminLogin()
        {
            return View();
        }

        // POST: Login/UserLogin
        [HttpPost]
		public async Task<IActionResult> UserLogin(string debitCardNumber, string pin)
		{
			// Step 1: Find the debit card by number and pin
			var debitCard = await _context.DebitCards
				.Where(dc => dc.CardNumber == debitCardNumber && dc.PIN == pin)
				.FirstOrDefaultAsync();

			if (debitCard == null)
			{
                // Debit card not found or invalid credentials
                ViewBag.ErrorMessage = "Invalid login attempt.";
                return View("LogIn");
            }
			else
			{
                // Debit card not found or invalid credentials
                ViewBag.ErrorMessage = "Welcome.";
                return View("Login");
            }

			// Step 2: Find the related Account for the DebitCard
			var account = await _context.Accounts
				.Where(a => a.Id == debitCard.AccountId)
				.FirstOrDefaultAsync();

			if (account == null )
			{
                // Account not found or invalid association
                ViewBag.ErrorMessage = "Invalid login attempt.";
                return View("LogIn");
            }

			// Step 3: Find the related User for the Account
			var user = await _context.Users
				.Where(u => u.Id == account.UserId)
				.FirstOrDefaultAsync();

			if (user == null)
			{
                // User not found or invalid association
                ViewBag.ErrorMessage = "Invalid login attempt.";
                return View("LogIn");
            }
			return View();
		}

		
	

		// POST: Login/AdminLogin
		[HttpPost]
		public async Task<IActionResult> AdminLogin(string Email, string Password)
		{
			// Find the admin by email and password
			

			if (Email==email && Password== password)
			{
                return RedirectToAction("Index", "Home");
            }
			else
			{
                // Admin not found or invalid credentials
                ViewBag.ErrorMessage = "Invalid login attempt.";
                return View("LogIn");
            }

            // Set session or claims-based authentication for Admin
           // HttpContext.Session.SetString("UserRole", "Admin");
			//HttpContext.Session.SetInt32("UserId", admin.Id);
			//return RedirectToAction("AdminDashboard", "Admin");
		}
	}
}






