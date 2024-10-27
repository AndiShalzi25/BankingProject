using BankingProject.Models;
using BankingProject.Models.Payment;
using Microsoft.EntityFrameworkCore;

namespace BankingProject.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Deposit> Deposits { get; set; }
		public DbSet<DebitCard> DebitCards { get; set; }
		public DbSet<Mobile> Mobiles { get; set; }
		public DbSet<CarTicket> CarTickets { get; set; }
		public DbSet<Transfer> Transfers { get; set; }
		public DbSet<ExchangeRate> ExchangeRates { get; set; }
		public DbSet<WithdrawAndDeposit> WithdrawAndDeposits { get; set; }
	}
}
