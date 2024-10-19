using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingProject.Migrations
{
    /// <inheritdoc />
    public partial class addReceiversNameToTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverFullName",
                table: "Transfers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverFullName",
                table: "Transfers");
        }
    }
}
