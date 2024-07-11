using BankingProject.Models;

namespace BankingProject.DTO
{
	public class UserIndexViewModel
	{
		public User User { get; set; }
		public List<Account> Accounts { get; set; }
	}
}
