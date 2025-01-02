using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManagement.Migrations
{
    /// <inheritdoc />
    public partial class V6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 15m);

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 25m);

            migrationBuilder.InsertData(
                table: "TicketTypes",
                columns: new[] { "Id", "AvailableQuantity", "Name", "Price" },
                values: new object[] { 4, 0, "Female - Standard Release", 25.00m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 100m);

            migrationBuilder.UpdateData(
                table: "TicketTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 100m);
        }
    }
}
