using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPesagemNovo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pesagens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    AtletaId = table.Column<int>(type: "int", nullable: false),
                    Pesagem1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pesagem2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pesagem3 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Peso1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Peso2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Peso3 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Obs1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Obs2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Obs3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Categoria1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Categoria2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Categoria3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Categoria4 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pesagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pesagens_Atletas_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Atletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pesagens_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pesagens_AtletaId",
                table: "Pesagens",
                column: "AtletaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pesagens_EventoId",
                table: "Pesagens",
                column: "EventoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pesagens");
        }
    }
}
