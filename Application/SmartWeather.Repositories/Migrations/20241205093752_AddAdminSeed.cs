using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWeather.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "admin@smartweather.net", "749F09BADE8ACA755660EEB17792DA880218D4FBDC4E25FBEC279D7FE9F65D70", 1, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
