using BankingProject.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingProject.Models
{
	public class WithdrawAndDeposit : BaseEntity
	{
		[Required]
		public bool IsWithdraw { get; set; }

		[Required]
		public double Amount { get; set; }

		[Required]
		public string Description { get; set; }

		[ForeignKey("DepositId")]
		public int DepositId { get; set; }

		public virtual Deposit Deposit { get; set; }
	}
}
