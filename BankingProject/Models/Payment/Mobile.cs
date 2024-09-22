using BankingProject.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingProject.Models.Payment
{
	public class Mobile : BaseEntity
	{
		[Required]
		public string PhoneNo { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[ForeignKey("DepositId")]
		public int? DepositId { get; set; }

		[Required]
		public double Amount { get; set; }

		[Required]
		public string DepositName { get; set; }

		[Required]
		public string Currency { get; set; }

		[ForeignKey("UserId")]
		public int UserId { get; set; }

		public virtual User User { get; set; }
	}
}
