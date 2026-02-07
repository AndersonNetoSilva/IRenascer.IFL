using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class addAvaliacaoArquivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArquivoImagemId",
                table: "AvaliacoesFuncionais",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Arquivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeOriginal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conteudo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Tamanho = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arquivos", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesFuncionais_Arquivos_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.DropTable(
                name: "Arquivos");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesFuncionais_ArquivoImagemId",
                table: "AvaliacoesFuncionais");

            migrationBuilder.DropColumn(
                name: "ArquivoImagemId",
                table: "AvaliacoesFuncionais");
        }
    }
}
