using BankingProject.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingProject.Models
{
	public class Transfer : BaseEntity
	{
		public string Title { get; set; }

		public string Description { get; set; }

		public string SWIFT { get; set; }

		public string IBAN { get; set; }

		public double Amount { get; set; }

		public string Currency { get; set; }

		[ForeignKey("UserId")]
		public int? UserId { get; set; }

		public virtual User User { get; set; }

		//UserId
		public int ReceiverId { get; set; }

		public string ReceiverFullName { get; set; }

		[ForeignKey("DepositId")]
		public int DepositId { get; set; }

		public virtual Deposit Deposit { get; set; }
	}
}
