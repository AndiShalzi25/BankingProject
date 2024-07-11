using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingProject.Models
{
	public class UserRole
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("UserId")]
		public int UserId { get; set; }
		public virtual User User { get; set; }
		[ForeignKey("RoleId")]
		public int RoleId { get; set; }
		public virtual Role Role { get; set; }

	}
}
