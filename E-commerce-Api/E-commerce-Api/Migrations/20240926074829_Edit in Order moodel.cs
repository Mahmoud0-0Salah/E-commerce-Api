using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookApp.Migrations
{
    /// <inheritdoc />
    public partial class EditinOrdermoodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d801c64-e5c8-4df3-ac34-84b36969e128");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c978a4e8-6507-42b4-816f-4294db4d389a");

            migrationBuilder.AlterColumn<int>(
                name: "OrderState",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04d66dd4-cf52-4791-a7bf-761b083d862d", null, "User", "USER" },
                    { "06372b97-aca0-419c-9330-c49dd5740b4f", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04d66dd4-cf52-4791-a7bf-761b083d862d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06372b97-aca0-419c-9330-c49dd5740b4f");

            migrationBuilder.AlterColumn<string>(
                name: "OrderState",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6d801c64-e5c8-4df3-ac34-84b36969e128", null, "Admin", "ADMIN" },
                    { "c978a4e8-6507-42b4-816f-4294db4d389a", null, "User", "USER" }
                });
        }
    }
}
