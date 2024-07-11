using System.ComponentModel.DataAnnotations;

namespace BankingProject.Models
{
	public class Role
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual List<UserRole> UserRoles { get; set; }
	}
}
