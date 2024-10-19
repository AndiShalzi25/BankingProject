using System.ComponentModel.DataAnnotations;

namespace BankingProject.Models
{
    public class ExchangeRate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FetchDate { get; set; }

        [Required]
        public double Lek { get; set; }

        [Required]
        public double Euro { get; set; }

        [Required]
        public double Dollar { get; set; }

        [Required]
        public double Pound { get; set; }

        [Required]
        public string Currency { get; set; }
    }
}
