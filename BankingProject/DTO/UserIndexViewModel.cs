using BankingProject.Models;
using BankingProject.Models.Payment;

namespace BankingProject.DTO
{
	public class UserIndexViewModel
	{
		public User User { get; set; }
		public List<Account> Accounts { get; set; }
		public List<Deposit> Deposits { get; set; }
		public List<Mobile> Mobiles { get; set; }
		public List<CarTicket> CarTickets { get; set; }
	}
}
