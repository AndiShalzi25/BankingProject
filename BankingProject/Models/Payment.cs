using BankingProject.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingProject.Models
{
	public class Payment : BaseEntity
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public DateTime PaymentDate { get; set; }

		[Required]
		public string DepositName { get; set; }

		[Required]
		public double Amount { get; set; }

		[Required]
		public string Currency { get; set; }

		[ForeignKey("UserId")]
		public int UserId { get; set; }

		public virtual User User { get; set; }
	}
}
