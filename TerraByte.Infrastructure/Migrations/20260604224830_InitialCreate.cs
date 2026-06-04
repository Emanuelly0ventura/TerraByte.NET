using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerraByte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Genero = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FotoPerfil = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_FarmPlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Cep = table.Column<string>(type: "TEXT", maxLength: 9, nullable: false),
                    Street = table.Column<string>(type: "TEXT", maxLength: 180, nullable: false),
                    District = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    SoilName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Clay = table.Column<double>(type: "REAL", nullable: false),
                    Sand = table.Column<double>(type: "REAL", nullable: false),
                    Silt = table.Column<double>(type: "REAL", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SoilRadiusKm = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_FarmPlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_FarmPlots_TB_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_Crops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    SeedName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    PlantingDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    FarmPlotId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Crops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Crops_TB_FarmPlots_FarmPlotId",
                        column: x => x.FarmPlotId,
                        principalTable: "TB_FarmPlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ResearchSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Kind = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FarmPlotId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ResearchSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ResearchSnapshots_TB_FarmPlots_FarmPlotId",
                        column: x => x.FarmPlotId,
                        principalTable: "TB_FarmPlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Crops_FarmPlotId",
                table: "TB_Crops",
                column: "FarmPlotId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_FarmPlots_UserId",
                table: "TB_FarmPlots",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ResearchSnapshots_FarmPlotId",
                table: "TB_ResearchSnapshots",
                column: "FarmPlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Crops");

            migrationBuilder.DropTable(
                name: "TB_ResearchSnapshots");

            migrationBuilder.DropTable(
                name: "TB_FarmPlots");

            migrationBuilder.DropTable(
                name: "TB_Users");
        }
    }
}
