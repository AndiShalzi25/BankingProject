using BankingProject.Models;

namespace BankingProject.DTO
{
	public class UserIndexViewModel
	{
		public User User { get; set; }
		public List<Account> Accounts { get; set; }
		public List<Payment> Payments { get; set; }
		public List<Deposit> Deposits { get; set; }
	}
}
