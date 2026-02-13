using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAjusteNomeClasse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesFuncionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesFuncionais_Atletas_AtletaId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvaliacoesFuncionais",
                table: "AvaliacoesFuncionais");

            migrationBuilder.RenameTable(
                name: "AvaliacoesFuncionais",
                newName: "AvaliacoesNutricionais");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesFuncionais_AtletaId",
                table: "AvaliacoesNutricionais",
                newName: "IX_AvaliacoesNutricionais_AtletaId");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesNutricionais",
                newName: "IX_AvaliacoesNutricionais_ArquivoImagemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvaliacoesNutricionais",
                table: "AvaliacoesNutricionais",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesNutricionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesNutricionais",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesNutricionais_Atletas_AtletaId",
                table: "AvaliacoesNutricionais",
                column: "AtletaId",
                principalTable: "Atletas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesNutricionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesNutricionais");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesNutricionais_Atletas_AtletaId",
                table: "AvaliacoesNutricionais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvaliacoesNutricionais",
                table: "AvaliacoesNutricionais");

            migrationBuilder.RenameTable(
                name: "AvaliacoesNutricionais",
                newName: "AvaliacoesFuncionais");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesNutricionais_AtletaId",
                table: "AvaliacoesFuncionais",
                newName: "IX_AvaliacoesFuncionais_AtletaId");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesNutricionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                newName: "IX_AvaliacoesFuncionais_ArquivoImagemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvaliacoesFuncionais",
                table: "AvaliacoesFuncionais",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesFuncionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesFuncionais_Atletas_AtletaId",
                table: "AvaliacoesFuncionais",
                column: "AtletaId",
                principalTable: "Atletas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
