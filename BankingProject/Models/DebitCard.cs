using BankingProject.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingProject.Models
{
	public class DebitCard : BaseEntity
	{
		[Required]
		[MaxLength(10)]
		[MinLength(10)]
		public string SerialNumber {  get; set; }
		[Required]
		[MaxLength(16)]
		[MinLength(16)]
		public string CardNumber {  get; set; }
		[Required]
		[MaxLength(3)]
		[MinLength(3)]
		public string CCV {  get; set; }
		[Required]
		public string Name {  get; set; }
		[Required]
		public DateTime ExpireDate { get; set; }

		[ForeignKey("AccountId")]
		public int AccountId { get; set; }
		public virtual Account Account { get; set; }
	}
}
