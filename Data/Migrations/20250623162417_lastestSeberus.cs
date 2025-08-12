using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serberus_Racket_Store.Migrations
{
    /// <inheritdoc />
    public partial class lastestSeberus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "brand",
                table: "Rackets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "Rackets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
