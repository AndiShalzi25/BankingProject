using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingProject.Migrations
{
    /// <inheritdoc />
    public partial class PinAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PIN",
                table: "DebitCards",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PIN",
                table: "DebitCards");
        }
    }
}
