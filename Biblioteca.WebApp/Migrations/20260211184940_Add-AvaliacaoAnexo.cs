using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAvaliacaoAnexo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacaoNutricionalAnexo_Arquivos_ArquivoImagemId",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacaoNutricionalAnexo_AvaliacoesNutricionais_AvaliacaoNutricionalId",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacaoNutricionalAnexo_ArquivoImagemId",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.DropColumn(
                name: "DescricaoImagem",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.RenameColumn(
                name: "ArquivoImagemId",
                table: "AvaliacaoNutricionalAnexo",
                newName: "Tipo");

            migrationBuilder.AlterColumn<int>(
                name: "AvaliacaoNutricionalId",
                table: "AvaliacaoNutricionalAnexo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnexoId",
                table: "AvaliacaoNutricionalAnexo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "AvaliacaoNutricionalAnexo",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoNutricionalAnexo_AnexoId",
                table: "AvaliacaoNutricionalAnexo",
                column: "AnexoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacaoNutricionalAnexo_Arquivos_AnexoId",
                table: "AvaliacaoNutricionalAnexo",
                column: "AnexoId",
                principalTable: "Arquivos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacaoNutricionalAnexo_AvaliacoesNutricionais_AvaliacaoNutricionalId",
                table: "AvaliacaoNutricionalAnexo",
                column: "AvaliacaoNutricionalId",
                principalTable: "AvaliacoesNutricionais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacaoNutricionalAnexo_Arquivos_AnexoId",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacaoNutricionalAnexo_AvaliacoesNutricionais_AvaliacaoNutricionalId",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacaoNutricionalAnexo_AnexoId",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.DropColumn(
                name: "AnexoId",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "AvaliacaoNutricionalAnexo");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "AvaliacaoNutricionalAnexo",
                newName: "ArquivoImagemId");

            migrationBuilder.AlterColumn<int>(
                name: "AvaliacaoNutricionalId",
                table: "AvaliacaoNutricionalAnexo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "DescricaoImagem",
                table: "AvaliacaoNutricionalAnexo",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoNutricionalAnexo_ArquivoImagemId",
                table: "AvaliacaoNutricionalAnexo",
                column: "ArquivoImagemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacaoNutricionalAnexo_Arquivos_ArquivoImagemId",
                table: "AvaliacaoNutricionalAnexo",
                column: "ArquivoImagemId",
                principalTable: "Arquivos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacaoNutricionalAnexo_AvaliacoesNutricionais_AvaliacaoNutricionalId",
                table: "AvaliacaoNutricionalAnexo",
                column: "AvaliacaoNutricionalId",
                principalTable: "AvaliacoesNutricionais",
                principalColumn: "Id");
        }
    }
}
