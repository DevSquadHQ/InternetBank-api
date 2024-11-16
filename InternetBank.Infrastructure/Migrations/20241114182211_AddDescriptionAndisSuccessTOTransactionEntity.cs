using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetBank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndisSuccessTOTransactionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSuccess",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "isSuccess",
                table: "Transactions");
        }
    }
}
