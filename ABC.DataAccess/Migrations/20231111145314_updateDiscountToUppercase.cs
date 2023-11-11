using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateDiscountToUppercase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discount",
                table: "OrderDetails",
                newName: "Discount");

            migrationBuilder.UpdateData(
                table: "UsersManagement",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 11, 22, 53, 14, 354, DateTimeKind.Local).AddTicks(5577));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "OrderDetails",
                newName: "discount");

            migrationBuilder.UpdateData(
                table: "UsersManagement",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 11, 21, 39, 55, 743, DateTimeKind.Local).AddTicks(7911));
        }
    }
}
