using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serberus_Racket_Store.Migrations
{
    /// <inheritdoc />
    public partial class newSeberus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONCAT('US', FORMAT(userId, 'D2'))");

            migrationBuilder.AddColumn<string>(
                name: "shippingCode",
                table: "Shippinginfo",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONCAT('SH', FORMAT(shippingId, 'D2'))");

            migrationBuilder.AddColumn<string>(
                name: "reviewCode",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONCAT('RE', FORMAT(reviewId, 'D2'))");

            migrationBuilder.AddColumn<string>(
                name: "racketCode",
                table: "Rackets",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONCAT('RA', FORMAT(racketId, 'D2'))");

            migrationBuilder.AddColumn<string>(
                name: "orderCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONCAT('OR', FORMAT(orderId, 'D2'))");

            migrationBuilder.AddColumn<string>(
                name: "orderItemCode",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONCAT('OI', FORMAT(orderItemId, 'D2'))");

            migrationBuilder.AddColumn<string>(
                name: "cartItemCode",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONCAT('CA', FORMAT(cartItemId, 'D2'))");

            migrationBuilder.AddColumn<string>(
                name: "brandCode",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONCAT('US', FORMAT(brandId, 'D2'))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "shippingCode",
                table: "Shippinginfo");

            migrationBuilder.DropColumn(
                name: "reviewCode",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "racketCode",
                table: "Rackets");

            migrationBuilder.DropColumn(
                name: "orderCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "orderItemCode",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "cartItemCode",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "brandCode",
                table: "Brands");
        }
    }
}
