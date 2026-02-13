using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAnexoAjuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvaliacaoNutricionalAnexo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvaliacaoNutricionalId = table.Column<int>(type: "int", nullable: true),
                    DescricaoImagem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ArquivoImagemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacaoNutricionalAnexo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacaoNutricionalAnexo_Arquivos_ArquivoImagemId",
                        column: x => x.ArquivoImagemId,
                        principalTable: "Arquivos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AvaliacaoNutricionalAnexo_AvaliacoesNutricionais_AvaliacaoNutricionalId",
                        column: x => x.AvaliacaoNutricionalId,
                        principalTable: "AvaliacoesNutricionais",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoNutricionalAnexo_ArquivoImagemId",
                table: "AvaliacaoNutricionalAnexo",
                column: "ArquivoImagemId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoNutricionalAnexo_AvaliacaoNutricionalId",
                table: "AvaliacaoNutricionalAnexo",
                column: "AvaliacaoNutricionalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvaliacaoNutricionalAnexo");
        }
    }
}
