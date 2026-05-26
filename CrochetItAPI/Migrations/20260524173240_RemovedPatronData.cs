using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrochetItAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPatronData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatronData",
                table: "Patrones",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Patrones",
                newName: "PatronData");
        }
    }
}
