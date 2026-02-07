using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class addAvaliacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvaliacoesFuncionais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AtletaId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Altura = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    Peso = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PctGordura = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    MassaMuscular = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    MassaLivre = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    GorduraViceral = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    AguaCorporal = table.Column<decimal>(type: "decimal(6,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacoesFuncionais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacoesFuncionais_Atletas_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Atletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesFuncionais_AtletaId",
                table: "AvaliacoesFuncionais",
                column: "AtletaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvaliacoesFuncionais");
        }
    }
}
