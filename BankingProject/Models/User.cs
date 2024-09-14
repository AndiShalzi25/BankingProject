using BankingProject.Models.Base;
using System.ComponentModel.DataAnnotations;

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
		public virtual List<Payment> Payments { get; set; }
	}
}
