using System.ComponentModel.DataAnnotations;

namespace BankingProject.Models.Base
{
	public class BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int CreatedById { get; set; }

	}
}
