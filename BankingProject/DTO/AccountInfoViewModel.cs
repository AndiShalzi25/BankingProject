using BankingProject.Models;

namespace BankingProject.DTO
{
	public class AccountInfoViewModel
	{
		public Account Account { get; set; }
		public List<Deposit> Deposits { get; set; }
		public List<DebitCard> DebitCards { get; set; }
		public string UserFullName { get; set; }
	}
}
