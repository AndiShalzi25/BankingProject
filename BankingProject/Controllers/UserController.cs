using BankingProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankingProject.Controllers
{
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _context;

		public UserController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
