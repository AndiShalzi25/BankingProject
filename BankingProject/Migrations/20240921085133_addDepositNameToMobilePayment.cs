using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingProject.Migrations
{
    /// <inheritdoc />
    public partial class addDepositNameToMobilePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepositName",
                table: "Mobiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositName",
                table: "Mobiles");
        }
    }
}
