using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEstatisticaCompeticao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstatisticasCompeticao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    AtletaId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatisticasCompeticao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstatisticasCompeticao_Atletas_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Atletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstatisticasCompeticao_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstatisticaCompeticaoDetalhe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstatisticaCompeticaoId = table.Column<int>(type: "int", nullable: false),
                    Vitoria = table.Column<bool>(type: "bit", nullable: true),
                    Yuko = table.Column<int>(type: "int", nullable: true),
                    Wazari = table.Column<int>(type: "int", nullable: true),
                    Ippon = table.Column<int>(type: "int", nullable: true),
                    Shido = table.Column<int>(type: "int", nullable: true),
                    Hansokumake = table.Column<int>(type: "int", nullable: true),
                    GoldenScore = table.Column<bool>(type: "bit", nullable: true),
                    TempoDaLuta = table.Column<TimeSpan>(type: "time", nullable: false),
                    TempoDoGoldenScore = table.Column<TimeSpan>(type: "time", nullable: false),
                    TecnicaAplicou = table.Column<int>(type: "int", nullable: false),
                    TecnicaRecebeu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatisticaCompeticaoDetalhe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstatisticaCompeticaoDetalhe_EstatisticasCompeticao_EstatisticaCompeticaoId",
                        column: x => x.EstatisticaCompeticaoId,
                        principalTable: "EstatisticasCompeticao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstatisticaCompeticaoDetalhe_EstatisticaCompeticaoId",
                table: "EstatisticaCompeticaoDetalhe",
                column: "EstatisticaCompeticaoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstatisticasCompeticao_AtletaId",
                table: "EstatisticasCompeticao",
                column: "AtletaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstatisticasCompeticao_EventoId",
                table: "EstatisticasCompeticao",
                column: "EventoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstatisticaCompeticaoDetalhe");

            migrationBuilder.DropTable(
                name: "EstatisticasCompeticao");
        }
    }
}
