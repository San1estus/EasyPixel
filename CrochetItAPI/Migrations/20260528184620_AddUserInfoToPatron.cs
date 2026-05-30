using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrochetItAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoToPatron : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Patrones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Patrones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patrones");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Patrones");
        }
    }
}
