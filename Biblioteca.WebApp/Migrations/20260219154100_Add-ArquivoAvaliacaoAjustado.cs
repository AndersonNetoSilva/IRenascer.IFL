using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddArquivoAvaliacaoAjustado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesNutricionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesNutricionais");

            migrationBuilder.AddColumn<int>(
                name: "ArquivoImagemCostasId",
                table: "AvaliacoesNutricionais",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesNutricionais_ArquivoImagemCostasId",
                table: "AvaliacoesNutricionais",
                column: "ArquivoImagemCostasId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesNutricionais_Arquivos_ArquivoImagemCostasId",
                table: "AvaliacoesNutricionais",
                column: "ArquivoImagemCostasId",
                principalTable: "Arquivos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesNutricionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesNutricionais",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesNutricionais_Arquivos_ArquivoImagemCostasId",
                table: "AvaliacoesNutricionais");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesNutricionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesNutricionais");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesNutricionais_ArquivoImagemCostasId",
                table: "AvaliacoesNutricionais");

            migrationBuilder.DropColumn(
                name: "ArquivoImagemCostasId",
                table: "AvaliacoesNutricionais");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesNutricionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesNutricionais",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
