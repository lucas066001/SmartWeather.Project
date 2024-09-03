using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWeather.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ComponentData_ComponentId",
                table: "ComponentData");

            migrationBuilder.AddColumn<int>(
                name: "GpioPin",
                table: "Component",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentData_ComponentId_DateTime",
                table: "ComponentData",
                columns: new[] { "ComponentId", "DateTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ComponentData_ComponentId_DateTime",
                table: "ComponentData");

            migrationBuilder.DropColumn(
                name: "GpioPin",
                table: "Component");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentData_ComponentId",
                table: "ComponentData",
                column: "ComponentId");
        }
    }
}
