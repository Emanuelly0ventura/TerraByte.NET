using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerraByte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFarmPlotSoilTexture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoilClassification",
                table: "TB_FarmPlots");

            migrationBuilder.AddColumn<double>(
                name: "Clay",
                table: "TB_FarmPlots",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Sand",
                table: "TB_FarmPlots",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Silt",
                table: "TB_FarmPlots",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "SoilName",
                table: "TB_FarmPlots",
                type: "TEXT",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "SoilRadiusKm",
                table: "TB_FarmPlots",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clay",
                table: "TB_FarmPlots");

            migrationBuilder.DropColumn(
                name: "Sand",
                table: "TB_FarmPlots");

            migrationBuilder.DropColumn(
                name: "Silt",
                table: "TB_FarmPlots");

            migrationBuilder.DropColumn(
                name: "SoilName",
                table: "TB_FarmPlots");

            migrationBuilder.DropColumn(
                name: "SoilRadiusKm",
                table: "TB_FarmPlots");

            migrationBuilder.AddColumn<string>(
                name: "SoilClassification",
                table: "TB_FarmPlots",
                type: "TEXT",
                maxLength: 120,
                nullable: true);
        }
    }
}
