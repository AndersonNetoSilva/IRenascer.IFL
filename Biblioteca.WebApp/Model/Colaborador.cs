using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class Colaborador : EntityBase
    {
        [Required]
        [StringLength(60)]
        [Display(Name = "Nome")]
        public required string Nome { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public required DateTime DataNascimento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Inicio de Atividade")]
        public required DateTime InicioAtividade { get; set; }

        [Required]
        [StringLength(16)]
        [Display(Name = "CPF")]
        public required string CPF { get; set; }

        [StringLength(12)]
        [Display(Name = "Registro Geral")]
        public  string? RG { get; set; }

        [StringLength(40)]
        [Display(Name = "E-mail")]
        public  string? Email { get; set; }

        [StringLength(16)]
        [Display(Name = "Telefone Principal")]
        public string? TelefonePrincipal { get; set; }

        [StringLength(30)]
        [Display(Name = "Profissão")]
        public string? Profissao { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Logradouro")]
        public string? Logradouro { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Município")]
        public required string Municipio { get; set; }

        [StringLength(10)]
        [Display(Name = "CEP")]
        public string? CEP { get; set; }

        [Display(Name = "Tipo de Colaborador")]
        public TipoColaborador? TipoColaborador { get; set; }
         
        [Display(Name = "Colaborador Ativo")]
        public bool Ativo { get; set; } = true;

        [NotMapped]
        [Display(Name = "Idade")]
        public int Idade
        {
            get
            {
                var hoje = DateTime.Today;
                var idade = hoje.Year - DataNascimento.Year;

                // ainda não fez aniversário neste ano?
                if (DataNascimento.Date > hoje.AddYears(-idade))
                    idade--;

                return idade;
            }
        }
    }

    public enum TipoColaborador
    {
        [Description("Auxiliar")]
        Auxiliar = 0,
        [Description("Sensei")]
        Sensei = 1,
        [Description("Nutricionista")]
        Nutricionista = 2,
        [Description("Gestor")]
        Gestor = 3

    }
}
