using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlantMonitorring.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Species = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    PlantingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    UserRole = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    PlantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantSensorDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    SensorId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantSensorDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantSensorDatas_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantSensorDatas_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "Id", "Description", "Image", "IsActive", "Location", "Name", "PlantingDate", "Species" },
                values: new object[,]
                {
                    { 1, "A beautiful red rose.", "rose.jpg", true, "GreenHouseA", "Rose", new DateTime(2022, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rosa" },
                    { 2, "A vibrant yellow tulip.", "tulip.jpg", true, "GreenHouseB", "Tulip", new DateTime(2022, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tulipa" },
                    { 3, "A hardy cactus that thrives in dry conditions.", "cactus.jpg", true, "Indoor", "Cactus", new DateTime(2022, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cactaceae" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "UserName", "UserRole" },
                values: new object[,]
                {
                    { 100, "sara@yahoo.com", "Sara", "", "100", "Admin" },
                    { 101, "Ali@yahoo.com", "Ali", "", "101", "User" }
                });

            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "Id", "IsActive", "Location", "PlantId", "Type", "Unit" },
                values: new object[,]
                {
                    { 1, true, "GreenHouseA", 1, "Temperature", "Celsius" },
                    { 2, true, "GreenHouseA", 1, "Humidity", "Percentage" },
                    { 3, true, "GreenHouseA", 1, "Soil Moisture", "Percentage" },
                    { 4, true, "GreenHouseA", 1, "Light", "Lux" },
                    { 5, true, "GreenHouseB", 2, "Temperature", "Celsius" },
                    { 6, true, "GreenHouseB", 2, "Humidity", "Percentage" },
                    { 7, true, "GreenHouseB", 2, "Soil Moisture", "Percentage" },
                    { 8, true, "GreenHouseB", 2, "Light", "Lux" }
                });

            migrationBuilder.InsertData(
                table: "PlantSensorDatas",
                columns: new[] { "Id", "PlantId", "SensorId", "Timestamp", "Value" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 5, 5, 5, 5, 9, 0, DateTimeKind.Unspecified), 24.0 },
                    { 2, 1, 2, new DateTime(2025, 5, 5, 5, 5, 9, 0, DateTimeKind.Unspecified), 70.0 },
                    { 3, 1, 3, new DateTime(2025, 5, 5, 5, 5, 9, 0, DateTimeKind.Unspecified), 50.0 },
                    { 4, 1, 4, new DateTime(2025, 5, 5, 5, 5, 9, 0, DateTimeKind.Unspecified), 70.0 },
                    { 5, 2, 5, new DateTime(2025, 5, 5, 5, 5, 9, 0, DateTimeKind.Unspecified), 70.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantSensorDatas_PlantId",
                table: "PlantSensorDatas",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantSensorDatas_SensorId",
                table: "PlantSensorDatas",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_PlantId",
                table: "Sensors",
                column: "PlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlantSensorDatas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Plants");
        }
    }
}
