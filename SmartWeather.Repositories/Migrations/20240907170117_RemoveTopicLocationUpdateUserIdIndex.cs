using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWeather.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTopicLocationUpdateUserIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Station_UserId",
                table: "Station");

            migrationBuilder.CreateIndex(
                name: "IX_Station_UserId",
                table: "Station",
                column: "UserId",
                unique: false);

            migrationBuilder.DropColumn(
                name: "TopicLocation",
                table: "Station");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Station_UserId",
                table: "Station");

            migrationBuilder.CreateIndex(
                name: "IX_Station_UserId",
                table: "Station",
                column: "UserId",
                unique: true);

            migrationBuilder.AddColumn<string>(
                name: "TopicLocation",
                table: "Station",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
