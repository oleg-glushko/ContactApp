using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedContactsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Email", "First", "Last", "Phone" },
                values: new object[,]
                {
                    { 1, "john@example.com", "John", "Smith", "123-456-7890" },
                    { 2, "dcran@example.com", "Dana", "Crandith", "123-456-7890" },
                    { 3, "en@example.com", "Edith", "Neutvaar", "123-456-7890" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
