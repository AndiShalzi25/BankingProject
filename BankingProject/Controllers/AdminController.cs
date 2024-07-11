using BankingProject.Data;
using Microsoft.AspNetCore.Mvc;
using BankingProject.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BankingProject.DTO;

namespace BankingProject.Controllers
{
	public class AdminController : Controller
	{

		private readonly Random random = new Random();
		private const string numberChars = "0123456789";
		private const string letterChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string letterAndNumberChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		public string GenerateRandomString(int length, string param)
		{
			StringBuilder result = new StringBuilder(length);
			for (int i = 0; i < length; i++)
			{
				result.Append(param[random.Next(param.Length)]);
			}
			return result.ToString();
		}

		private readonly ApplicationDbContext _context;

		public AdminController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var users = _context.Users.ToList();
			ViewBag.Users = users;
			return View();
		}

		public IActionResult CreateUser() 
		{ 
			return View();
		}

		public async Task<IActionResult> CreateNewUser() 
		{
			//1.Create user

			var newUser = new User()
			{
				Name = "Jane",
				Surname = "Doe",
				Email = "jane@gmail.com",
				Password = "123456",
				CreatedAt = DateTime.Now,
				CreatedById = 1002
			};

			await _context.Users.AddAsync(newUser);
			await _context.SaveChangesAsync();

			var newUserRole = new UserRole()
			{
				UserId = newUser.Id,
				User = newUser,
				RoleId = 2
			};

			await _context.UserRoles.AddAsync(newUserRole);
			await _context.SaveChangesAsync();

			var newAccount = new Account()
			{
				Name = $"{newUser.Name} {newUser.Surname} Account",
				TotalBalance = 0,
				UserId = newUser.Id,
				User = newUser,
				CreatedAt = DateTime.Now,
				CreatedById = 1002
			};

			await _context.Accounts.AddAsync(newAccount);
			await _context.SaveChangesAsync();

			var newDebitCard = new DebitCard()
			{
				SerialNumber = GenerateRandomString(10, numberChars),
				CardNumber = GenerateRandomString(16, numberChars),
				CCV = GenerateRandomString(3, numberChars),
				Name = $"{newUser.Name} {newUser.Surname}",
				ExpireDate = newAccount.CreatedAt.AddYears(5),
				AccountId = newAccount.Id,
				Account = newAccount
			};

			await _context.DebitCards.AddAsync(newDebitCard);
			await _context.SaveChangesAsync();

			var depositCurrency = "ALL";
			var newDeposit = new Deposit()
			{
				Currency = depositCurrency,
				Name = $"{newUser.Name} {newUser.Surname} deposit {depositCurrency}",
				IBAN = $"{GenerateRandomString(31, letterAndNumberChars)}{depositCurrency}",
				SWIFT = "NCBTAAL",
				Balance = 0,
				AccountId = newAccount.Id,
				Account = newAccount,
				CreatedById = 1002,
				CreatedAt = DateTime.Now
			};

			await _context.Deposits.AddAsync(newDeposit);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> UserIndex(int id)
		{
			var user = await _context.Users.Where(u => u.Id == id).FirstAsync();
			var accounts = await _context.Accounts.Include(a => a.Deposits).Where(a => a.UserId == id).ToListAsync();


			var model = new UserIndexViewModel()
			{
				User = user,
				Accounts = accounts
			};

			ViewBag.Model = model;

			return View();
		}
	}
}
