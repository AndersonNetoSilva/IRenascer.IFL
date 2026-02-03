using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class ADDAtletasSexo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                table: "Atletas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Atletas");
        }
    }
}
