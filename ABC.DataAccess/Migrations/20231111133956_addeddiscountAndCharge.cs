using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addeddiscountAndCharge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Charge",
                table: "OrderDetails",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "discount",
                table: "OrderDetails",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "UsersManagement",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 11, 21, 39, 55, 743, DateTimeKind.Local).AddTicks(7911));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Charge",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "discount",
                table: "OrderDetails");

            migrationBuilder.UpdateData(
                table: "UsersManagement",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 6, 17, 43, 59, 4, DateTimeKind.Local).AddTicks(5116));
        }
    }
}
