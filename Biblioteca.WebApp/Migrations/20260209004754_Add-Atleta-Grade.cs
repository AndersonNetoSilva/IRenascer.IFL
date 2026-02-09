using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAtletaGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArquivoImagemId",
                table: "Atletas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AtletaGrade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AtletaId = table.Column<int>(type: "int", nullable: false),
                    HorarioId = table.Column<int>(type: "int", nullable: false),
                    ModalidadeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtletaGrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtletaGrade_Atletas_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Atletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtletaGrade_Horarios_HorarioId",
                        column: x => x.HorarioId,
                        principalTable: "Horarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtletaGrade_Modalidades_ModalidadeId",
                        column: x => x.ModalidadeId,
                        principalTable: "Modalidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atletas_ArquivoImagemId",
                table: "Atletas",
                column: "ArquivoImagemId");

            migrationBuilder.CreateIndex(
                name: "IX_AtletaGrade_AtletaId",
                table: "AtletaGrade",
                column: "AtletaId");

            migrationBuilder.CreateIndex(
                name: "IX_AtletaGrade_HorarioId",
                table: "AtletaGrade",
                column: "HorarioId");

            migrationBuilder.CreateIndex(
                name: "IX_AtletaGrade_ModalidadeId",
                table: "AtletaGrade",
                column: "ModalidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atletas_Arquivos_ArquivoImagemId",
                table: "Atletas",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atletas_Arquivos_ArquivoImagemId",
                table: "Atletas");

            migrationBuilder.DropTable(
                name: "AtletaGrade");

            migrationBuilder.DropIndex(
                name: "IX_Atletas_ArquivoImagemId",
                table: "Atletas");

            migrationBuilder.DropColumn(
                name: "ArquivoImagemId",
                table: "Atletas");
        }
    }
}
