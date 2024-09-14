using BankingProject.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace BankingProject.Models
{
	public class Deposit : BaseEntity
	{
		public string Name { get; set; }
		public string IBAN { get; set; }
		public string SWIFT { get; set; }
		public double Balance { get; set; }
		public string Currency { get; set; }
		public bool IsDeleted { get; set; }

		[ForeignKey("AccountId")]
		public int AccountId { get; set; }
		public virtual Account Account { get; set; }
	}
}
