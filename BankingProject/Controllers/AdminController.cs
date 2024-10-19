using BankingProject.Data;
using Microsoft.AspNetCore.Mvc;
using BankingProject.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BankingProject.DTO;
using System.Security.Principal;
using BankingProject.Models.Payment;

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

		public async Task<IActionResult> Index()
		{
			var users = await _context.Users.ToListAsync();
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
				IsDeleted = false,
				AccountId = newAccount.Id,
				Account = newAccount,
				CreatedById = 1002,
				CreatedAt = DateTime.Now
			};

			await _context.Deposits.AddAsync(newDeposit);
			await _context.SaveChangesAsync();

			newAccount.Deposits.Add(newDeposit);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> UserIndex(int id, string tab)
		{
			if (tab == null || tab == "")
				tab = "ProfileTab";

			var user = await _context.Users.Where(u => u.Id == id).FirstAsync();
			var accounts = await _context.Accounts.Include(a => a.Deposits).Where(a => a.UserId == id).ToListAsync();

			var depositList = new List<Deposit>();

			foreach(var account in accounts)
			{
				foreach(var deposit in account.Deposits)
				{
					depositList.Add(deposit);
				}
			}

			var mobilePayments = await _context.Mobiles.Where(m => m.UserId == id).ToListAsync();

			var carTicketPayments = await _context.CarTickets.Where(c => c.UserId == id).ToListAsync();

			var payments = new List<Payment>();

			foreach(var payment in mobilePayments)
			{
				var p = new Payment()
				{
					PaymentId = payment.Id,
					Title = payment.Name,
					Description = payment.Description,
					Category = "Mobile",
					PaymentDate = payment.CreatedAt,
					Amount = payment.Amount,
					Currency = payment.Currency,
					DepositName = payment.DepositName
				};

				payments.Add(p);
			}

			foreach (var payment in carTicketPayments)
			{
				var p = new Payment()
				{
					PaymentId = payment.Id,
					Title = payment.Name,
					Description = payment.Description,
					Category = "Car Ticket",
					PaymentDate = payment.CreatedAt,
					Amount = payment.Amount,
					Currency = payment.Currency,
					DepositName = payment.DepositName
				};

				payments.Add(p);
			}


			var transfers = await _context.Transfers.Where(t => t.UserId == id).ToListAsync();

			var model = new UserIndexViewModel()
			{
				User = user,
				Accounts = accounts,
				Deposits = depositList,
				Mobiles = mobilePayments,
				CarTickets = carTicketPayments,
				Transfers = transfers
            };

			ViewBag.Model = model;
			ViewBag.PaymentList = payments;
			ViewData["ActiveTab"] = tab;

			return View();
		}


		public async Task<IActionResult> CreateAccount(int userId, string accountName)
		{

			var user = await _context.Users.Where(u => u.Id == userId).FirstAsync();

			var newAccount = new Account()
			{
				Name = accountName,
				TotalBalance = 0,
				UserId = user.Id,
				User = user,
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
				Name = $"{user.Name} {user.Surname}",
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
				Name = $"{user.Name} {user.Surname} deposit {depositCurrency}",
				IBAN = $"{GenerateRandomString(31, letterAndNumberChars)}{depositCurrency}",
				SWIFT = "NCBTAAL",
				Balance = 0,
				IsDeleted = false,
				AccountId = newAccount.Id,
				Account = newAccount,
				CreatedById = 1002,
				CreatedAt = DateTime.Now
			};

			await _context.Deposits.AddAsync(newDeposit);
			await _context.SaveChangesAsync();

			newAccount.Deposits.Add(newDeposit);
			await _context.SaveChangesAsync();

			return RedirectToAction("UserIndex", new { id = userId });
		}


		public async Task<IActionResult> EditAccount(int accountId, string accountName)
		{
			var accountToEdit = await _context.Accounts.Where(a => a.Id == accountId).FirstAsync();
			var userId = accountToEdit.UserId;
			accountToEdit.Name = accountName;

			await _context.SaveChangesAsync();

			return RedirectToAction("UserIndex", new { id = userId });
		}

		public async Task<IActionResult> DeleteAccount(int accountId)
		{
			var deposits = await _context.Deposits.Where(d => d.AccountId == accountId).ToListAsync();

			_context.Deposits.RemoveRange(deposits);
			await _context.SaveChangesAsync();

			var debitCards = await _context.DebitCards.Where(d => d.AccountId == accountId).ToListAsync();
			_context.DebitCards.RemoveRange(debitCards);
			await _context.SaveChangesAsync();

			var accountToRemove = await _context.Accounts.Where(a => a.Id == accountId).FirstAsync();

			var userId = accountToRemove.UserId;

			_context.Accounts.Remove(accountToRemove);
			await _context.SaveChangesAsync();

			return RedirectToAction("UserIndex", new { id = userId });
		}

		public async Task<IActionResult> AccountInfo(int accountId) 
		{
			var account = await _context.Accounts.Include(a => a.User).Where(a => a.Id == accountId).FirstAsync();
			var deposits = await _context.Deposits.Where(d => d.AccountId == accountId).ToListAsync();
			var debitCards = await _context.DebitCards.Where(d => d.AccountId == accountId).ToListAsync();

			var userFullName = $"{account.User.Name} {account.User.Surname}";

			var model = new AccountInfoViewModel()
			{
				Account = account,
				DebitCards = debitCards,
				Deposits = deposits,
				UserFullName = userFullName
			};

			ViewBag.AccountInfo = model;

			return View();
		}

		public async Task<IActionResult> CreateDeposit(int accountId, string depositName, string currency)
		{
			var account = await _context.Accounts.Where(a => a.Id == accountId).FirstAsync();

			var newDeposit = new Deposit()
			{
				Currency = currency,
				Name = depositName,
				IBAN = $"{GenerateRandomString(31, letterAndNumberChars)}{currency}",
				SWIFT = "NCBTAAL",
				Balance = 0,
				IsDeleted = false,
				AccountId = account.Id,
				Account = account,
				CreatedById = 1002,
				CreatedAt = DateTime.Now
			};

			await _context.Deposits.AddAsync(newDeposit);
			await _context.SaveChangesAsync();

			account.Deposits.Add(newDeposit);
			await _context.SaveChangesAsync();

			return RedirectToAction("AccountInfo", new { accountId = accountId });
		}

		public async Task<IActionResult> EditDeposit(int accountId, int depositId, string depositName)
		{
			var deposit = await _context.Deposits.Where(d => d.Id == depositId).FirstAsync();
			deposit.Name = depositName;

			await _context.SaveChangesAsync();
			return RedirectToAction("AccountInfo", new { accountId = accountId });
		}

		public async Task<IActionResult> DeleteDeposit(int accountId, int depositId)
		{
			var deposit = await _context.Deposits.Where(d => d.Id == depositId).FirstAsync();
			deposit.IsDeleted = true;

			await _context.SaveChangesAsync();

			return RedirectToAction("AccountInfo", new { accountId = accountId });
		}


		[HttpGet]
		public async Task<IActionResult> PaymentIndex(int userId)
		{
			var user = await _context.Users.Where(u => u.Id == userId).FirstAsync();
			
			var accounts = await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();

			var deposits = new List<Deposit>();

			foreach(var account in accounts)
			{
				var dep = await _context.Deposits.Where(d => d.AccountId == account.Id).ToListAsync();
				
				deposits.AddRange(dep);
			}

			
			ViewBag.User = user;
			ViewBag.Deposits = deposits;
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> CreateMobilePayment(string name, double amount, string phoneNo, int depositId, string description, int userId)
		{
			var deposit = await _context.Deposits.Where(d => d.Id == depositId).FirstOrDefaultAsync();

			var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

			var payment = new Mobile()
			{
				Name = name,
				Description = description,
				PhoneNo = phoneNo,
				DepositId = depositId,
				DepositName = deposit.Name,
				Currency = deposit.Currency,
				Amount = amount,
				UserId = userId,
				User = user,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				CreatedById = 1
			};

			await _context.Mobiles.AddAsync(payment);
			await _context.SaveChangesAsync();

			deposit.Balance -= amount;
			await _context.SaveChangesAsync();

			//ViewData["ActiveTab"] = "PaymentTab";

			return RedirectToAction("UserIndex", new { id = userId, tab = "PaymentTab" });
		}

		[HttpPost]
		public async Task<IActionResult> CreateCarTicketPayment(string name, double amount, string plate, int depositId, string description, int userId)
		{
			var deposit = await _context.Deposits.Where(d => d.Id == depositId).FirstOrDefaultAsync();

			var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

			var payment = new CarTicket()
			{
				Name = name,
				Description = description,
				Plate = plate,
				DepositId = depositId,
				DepositName = deposit.Name,
				Currency = deposit.Currency,
				Amount = amount,
				UserId = userId,
				User = user,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				CreatedById = 1
			};

			await _context.CarTickets.AddAsync(payment);
			await _context.SaveChangesAsync();

			deposit.Balance -= amount;
			await _context.SaveChangesAsync();

			//ViewData["ActiveTab"] = "PaymentTab";

			return RedirectToAction("UserIndex", new { id = userId, tab = "PaymentTab" });
		}

		[HttpPost]
		public async Task<IActionResult> CreateTransfer(int userId, string title, double amount, int depositId, string swift, string iban, string description) 
		{
			var sender = await _context.Users.Where(u => u.Id == userId).FirstAsync();
			//Senders
			var sendersDeposit = await _context.Deposits.Where(d => d.Id == depositId).FirstAsync();

			//Receiver
			var receiverDeposit = await _context.Deposits.Where(d => d.IBAN == iban).FirstAsync();
			var account = await _context.Accounts.Where(a => a.Id == receiverDeposit.AccountId).FirstAsync();

			var receiver = await _context.Users.Where(u => u.Id == account.UserId).FirstAsync();

			var newTransfer = new Transfer()
			{
				Title = title,
				Amount = amount,
				UserId = userId,
				User = sender,
				Currency = sendersDeposit.Currency,
				DepositId = sendersDeposit.Id,
				Deposit = sendersDeposit,
				IBAN = receiverDeposit.IBAN,
				SWIFT = receiverDeposit.SWIFT,
				ReceiverId = receiver.Id,
				ReceiverFullName = $"{receiver.Name} {receiver.Surname}",
				Description = description,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				CreatedById = 1
			};

			await _context.Transfers.AddAsync(newTransfer);
			await _context.SaveChangesAsync();

			sendersDeposit.Balance -= amount;
			await _context.SaveChangesAsync();

			var exhangeRate = await _context.ExchangeRates.Where(e => e.Currency == sendersDeposit.Currency).FirstAsync();

			if(receiverDeposit.Currency == "ALL")
                receiverDeposit.Balance += amount * exhangeRate.Lek;
            else if (receiverDeposit.Currency == "EUR")
                receiverDeposit.Balance += amount * exhangeRate.Euro;
            else if (receiverDeposit.Currency == "USD")
                receiverDeposit.Balance += amount * exhangeRate.Dollar;
            else if (receiverDeposit.Currency == "GBP")
                receiverDeposit.Balance += amount * exhangeRate.Pound;

            await _context.SaveChangesAsync();

			return RedirectToAction("UserIndex", new { id = userId, tab = "" });
		}
	}
}
