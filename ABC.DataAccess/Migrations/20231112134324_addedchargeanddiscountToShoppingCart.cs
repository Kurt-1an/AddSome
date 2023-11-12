using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedchargeanddiscountToShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Charge",
                table: "ShoppingCarts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "ShoppingCarts",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "UsersManagement",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 12, 21, 43, 24, 87, DateTimeKind.Local).AddTicks(3114));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Charge",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ShoppingCarts");

            migrationBuilder.UpdateData(
                table: "UsersManagement",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 11, 22, 53, 14, 354, DateTimeKind.Local).AddTicks(5577));
        }
    }
}
