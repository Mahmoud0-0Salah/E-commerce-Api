using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4434adfb-1659-4faa-9ab3-8dddb2703177");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca66d093-e620-4d97-b40f-e7ceb34222b3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e051a53-536c-40d7-b8f1-dc08502c6aa6", null, "Delivery", "DELIVERY" },
                    { "52094007-941e-4976-aea3-076d2df31690", null, "User", "USER" },
                    { "9135b65a-a0eb-4b71-a83b-b973f72deea8", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e051a53-536c-40d7-b8f1-dc08502c6aa6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52094007-941e-4976-aea3-076d2df31690");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9135b65a-a0eb-4b71-a83b-b973f72deea8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4434adfb-1659-4faa-9ab3-8dddb2703177", null, "Admin", "ADMIN" },
                    { "ca66d093-e620-4d97-b40f-e7ceb34222b3", null, "User", "USER" }
                });
        }
    }
}
