using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SmartWeather.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class WippingAllMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Station",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    MacAddress = table.Column<string>(type: "varchar(255)", nullable: false),
                    Latitude = table.Column<float>(type: "float", nullable: false),
                    Longitude = table.Column<float>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Station_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Color = table.Column<string>(type: "longtext", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    GpioPin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Component_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ActivationPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActivationTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    PeriodInDay = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivationPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivationPlan_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MeasurePoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LocalId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Color = table.Column<string>(type: "longtext", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurePoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurePoint_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "admin@smartweather.net", "749F09BADE8ACA755660EEB17792DA880218D4FBDC4E25FBEC279D7FE9F65D70", 1, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivationPlan_ComponentId",
                table: "ActivationPlan",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Component_StationId",
                table: "Component",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurePoint_ComponentId",
                table: "MeasurePoint",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Station_MacAddress",
                table: "Station",
                column: "MacAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Station_UserId",
                table: "Station",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivationPlan");

            migrationBuilder.DropTable(
                name: "MeasurePoint");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Station");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
