using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace BankingProject.Models.Payment
{
    public class Payment
    {
        [Required]
        public int PaymentId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string DepositName { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public string Category { get; set; }

    }
}
