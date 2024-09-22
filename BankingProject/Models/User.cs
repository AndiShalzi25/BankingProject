using BankingProject.Models.Base;
using BankingProject.Models.Payment;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace BankingProject.Models
{
	public class User : BaseEntity
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public virtual List<UserRole> UserRoles { get; set; }
		public virtual List<Account> Accounts { get; set; }
		public virtual List<Mobile> Mobiles { get; set; }
		public virtual List<CarTicket> CarTickets { get; set; }
	}
}
