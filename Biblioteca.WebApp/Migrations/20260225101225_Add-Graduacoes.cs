using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGraduacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Graduacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HorarioId = table.Column<int>(type: "int", nullable: false),
                    ModalidadeId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Encerrada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graduacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Graduacoes_Horarios_HorarioId",
                        column: x => x.HorarioId,
                        principalTable: "Horarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Graduacoes_Modalidades_ModalidadeId",
                        column: x => x.ModalidadeId,
                        principalTable: "Modalidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraduacaoAtleta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GraducaoId = table.Column<int>(type: "int", nullable: false),
                    Aprovado = table.Column<bool>(type: "bit", nullable: true),
                    AtletaId = table.Column<int>(type: "int", nullable: false),
                    NotaEscrita = table.Column<int>(type: "int", nullable: true),
                    NotaPratica = table.Column<int>(type: "int", nullable: true),
                    FaixaNova = table.Column<int>(type: "int", nullable: false),
                    ArquivoId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraduacaoAtleta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraduacaoAtleta_Arquivos_ArquivoId",
                        column: x => x.ArquivoId,
                        principalTable: "Arquivos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GraduacaoAtleta_Atletas_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Atletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraduacaoAtleta_Graduacoes_GraducaoId",
                        column: x => x.GraducaoId,
                        principalTable: "Graduacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraduacaoAtleta_ArquivoId",
                table: "GraduacaoAtleta",
                column: "ArquivoId");

            migrationBuilder.CreateIndex(
                name: "IX_GraduacaoAtleta_AtletaId",
                table: "GraduacaoAtleta",
                column: "AtletaId");

            migrationBuilder.CreateIndex(
                name: "IX_GraduacaoAtleta_GraducaoId",
                table: "GraduacaoAtleta",
                column: "GraducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Graduacoes_HorarioId",
                table: "Graduacoes",
                column: "HorarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Graduacoes_ModalidadeId",
                table: "Graduacoes",
                column: "ModalidadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraduacaoAtleta");

            migrationBuilder.DropTable(
                name: "Graduacoes");
        }
    }
}
