using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class ADDAtletasTelefoneResponsavel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelefoneResponsavel",
                table: "Atletas",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelefoneResponsavel",
                table: "Atletas");
        }
    }
}
