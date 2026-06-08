using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerraByte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteDefensivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Defensivo_terrabyte",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defensivo_terrabyte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plantio_terrabyte",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    TempMin = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    TempMax = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    AguaMM = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    MesesIdeais = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    UrlImg = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantio_terrabyte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoSolo_terrabyte",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoSolo_terrabyte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario_terrabyte",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Senha = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Genero = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "date", nullable: false),
                    FotoPerfil = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario_terrabyte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "plan_def_terrabyte",
                columns: table => new
                {
                    CulturaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefensivoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plan_def_terrabyte", x => new { x.CulturaId, x.DefensivoId });
                    table.ForeignKey(
                        name: "FK_plan_def_terrabyte_Defensivo_terrabyte_DefensivoId",
                        column: x => x.DefensivoId,
                        principalTable: "Defensivo_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plan_def_terrabyte_Plantio_terrabyte_CulturaId",
                        column: x => x.CulturaId,
                        principalTable: "Plantio_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plan_tp_terrabyte",
                columns: table => new
                {
                    CulturaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TipoSoloId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plan_tp_terrabyte", x => new { x.CulturaId, x.TipoSoloId });
                    table.ForeignKey(
                        name: "FK_plan_tp_terrabyte_Plantio_terrabyte_CulturaId",
                        column: x => x.CulturaId,
                        principalTable: "Plantio_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plan_tp_terrabyte_TipoSolo_terrabyte_TipoSoloId",
                        column: x => x.TipoSoloId,
                        principalTable: "TipoSolo_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnderecoPlantio_terrabyte",
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
                    Argila = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    Areia = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    Silte = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    RaioSoloKm = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    TipoSoloId = table.Column<Guid>(type: "TEXT", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecoPlantio_terrabyte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoPlantio_terrabyte_TipoSolo_terrabyte_TipoSoloId",
                        column: x => x.TipoSoloId,
                        principalTable: "TipoSolo_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EnderecoPlantio_terrabyte_Usuario_terrabyte_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalisePlantio_terrabyte",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TempMin = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    TempMax = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    UmidadeMed = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    ChuvaPrevistaMm = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    AdequadoPlantio = table.Column<double>(type: "REAL", precision: 8, scale: 2, nullable: false),
                    NivelRisco = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Recomendacao = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TerrenoAgricolaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CulturaId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalisePlantio_terrabyte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalisePlantio_terrabyte_EnderecoPlantio_terrabyte_TerrenoAgricolaId",
                        column: x => x.TerrenoAgricolaId,
                        principalTable: "EnderecoPlantio_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalisePlantio_terrabyte_Plantio_terrabyte_CulturaId",
                        column: x => x.CulturaId,
                        principalTable: "Plantio_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalisePlantio_terrabyte_Usuario_terrabyte_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario_terrabyte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalisePlantio_terrabyte_CulturaId",
                table: "AnalisePlantio_terrabyte",
                column: "CulturaId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalisePlantio_terrabyte_TerrenoAgricolaId",
                table: "AnalisePlantio_terrabyte",
                column: "TerrenoAgricolaId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalisePlantio_terrabyte_UsuarioId",
                table: "AnalisePlantio_terrabyte",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoPlantio_terrabyte_TipoSoloId",
                table: "EnderecoPlantio_terrabyte",
                column: "TipoSoloId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoPlantio_terrabyte_UsuarioId",
                table: "EnderecoPlantio_terrabyte",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_plan_def_terrabyte_DefensivoId",
                table: "plan_def_terrabyte",
                column: "DefensivoId");

            migrationBuilder.CreateIndex(
                name: "IX_plan_tp_terrabyte_TipoSoloId",
                table: "plan_tp_terrabyte",
                column: "TipoSoloId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoSolo_terrabyte_Nome",
                table: "TipoSolo_terrabyte",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_terrabyte_Email",
                table: "Usuario_terrabyte",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalisePlantio_terrabyte");

            migrationBuilder.DropTable(
                name: "plan_def_terrabyte");

            migrationBuilder.DropTable(
                name: "plan_tp_terrabyte");

            migrationBuilder.DropTable(
                name: "EnderecoPlantio_terrabyte");

            migrationBuilder.DropTable(
                name: "Defensivo_terrabyte");

            migrationBuilder.DropTable(
                name: "Plantio_terrabyte");

            migrationBuilder.DropTable(
                name: "TipoSolo_terrabyte");

            migrationBuilder.DropTable(
                name: "Usuario_terrabyte");
        }
    }
}
