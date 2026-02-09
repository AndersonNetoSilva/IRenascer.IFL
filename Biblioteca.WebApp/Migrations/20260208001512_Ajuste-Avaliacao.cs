using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AjusteAvaliacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                column: "ArquivoImagemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                column: "ArquivoImagemId",
                unique: true,
                filter: "[ArquivoImagemId] IS NOT NULL");
        }
    }
}
