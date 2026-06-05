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
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Cep = table.Column<string>(type: "TEXT", maxLength: 9, nullable: false),
                    Logradouro = table.Column<string>(type: "TEXT", maxLength: 180, nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    NomeSolo = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Argila = table.Column<double>(type: "REAL", nullable: false),
                    Areia = table.Column<double>(type: "REAL", nullable: false),
                    Silte = table.Column<double>(type: "REAL", nullable: false),
                    RaioSoloKm = table.Column<double>(type: "REAL", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_FarmPlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_FarmPlots_TB_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_Crops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    NomeSemente = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    DataPlantio = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    TerrenoAgricolaId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Crops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Crops_TB_FarmPlots_TerrenoAgricolaId",
                        column: x => x.TerrenoAgricolaId,
                        principalTable: "TB_FarmPlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ResearchSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Fonte = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Resumo = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    SolicitadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TerrenoAgricolaId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ResearchSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ResearchSnapshots_TB_FarmPlots_TerrenoAgricolaId",
                        column: x => x.TerrenoAgricolaId,
                        principalTable: "TB_FarmPlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Crops_TerrenoAgricolaId",
                table: "TB_Crops",
                column: "TerrenoAgricolaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_FarmPlots_UsuarioId",
                table: "TB_FarmPlots",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ResearchSnapshots_TerrenoAgricolaId",
                table: "TB_ResearchSnapshots",
                column: "TerrenoAgricolaId");
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
