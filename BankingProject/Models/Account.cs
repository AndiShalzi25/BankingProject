using BankingProject.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingProject.Models
{
	public class Account : BaseEntity
	{
		public string Name { get; set; }
		public double TotalBalance { get; set; }
		[ForeignKey("UserId")]
		public int UserId { get; set; }
		public virtual User User { get; set; }
		public virtual List<Deposit> Deposits { get; set; }
		public virtual List<DebitCard> DebitCards { get; set; }
	}
}
