using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrochetItAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patrones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Patrones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
