using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class addAvaliacaoArquivoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesFuncionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                column: "ArquivoImagemId",
                unique: true,
                filter: "[ArquivoImagemId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesFuncionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesFuncionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                column: "ArquivoImagemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesFuncionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id");
        }
    }
}
