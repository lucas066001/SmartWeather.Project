using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWeather.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalId",
                table: "MeasurePoint",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalId",
                table: "MeasurePoint");
        }
    }
}
