using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IFL.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAtletas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atletas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DataNascimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataMatricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    RG = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    TelefonePrincipal = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Profissao = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Logradouro = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Municipio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CEP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ProblemaSaude = table.Column<bool>(type: "bit", nullable: false),
                    DescricaoProblemaSaude = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    AcionarUrgencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Escola = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TipoEscola = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atletas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atletas");
        }
    }
}
