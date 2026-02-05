using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class Atleta : EntityBase
    {
        [Required]
        [StringLength(60)]
        [Display(Name = "Nome")]
        public required string Nome { get; set; }        

        [NotMapped]
        [Display(Name = "Nome com PCD")]
        public string? NomeComPCD {
            get
            {
                string nome = Nome;

                if (PCD)
                    nome = nome + "(*)";

                return nome;
            }
        }

        [Required]
        [StringLength(60)]
        [Display(Name = "Responsável")]
        public required string Responsavel { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Nascimento")]
        public required DateTime DataNascimento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Matrícula")]
        public required DateTime DataMatricula { get; set; }

        [Required]
        [StringLength(16)]
        [Display(Name = "CPF")]
        public required string CPF { get; set; }

        [StringLength(12)]
        [Display(Name = "Registro Geral")]
        public string? RG { get; set; }

        [StringLength(40)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(16)]
        [Display(Name = "Telefone Principal")]
        public string? TelefonePrincipal { get; set; }

        [StringLength(16)]
        [Display(Name = "Telefone Responsável")]
        public string? TelefoneResponsavel { get; set; }

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

        [Required(ErrorMessage = "Informe se existe problema de saúde")]
        [Display(Name = "Problema de Saúde")]
        public bool ProblemaSaude { get; set; } = false;
        
        [Display(Name = "Pessoa com Deficiência")]
        public bool PCD { get; set; } = false;

        [StringLength(60)]
        [Display(Name = "Descrição do Problema de Saúde")]
        public string? DescricaoProblemaSaude { get; set; }

        [StringLength(200)]
        [Display(Name = "Em caso de urgência, quem acionar / Telefone")]
        public string? AcionarUrgencia { get; set; }

        [StringLength(20)]
        [Display(Name = "Nome da Escola")]
        public required string Escola { get; set; }

        [Display(Name = "Tipo de Escola")]
        public required TipoEscola TipoEscola { get; set; }

        [Display(Name = "Atleta Ativo")]
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

        [NotMapped]
        [Display(Name = "Classe do Atleta")]
        public string Classe
        {
            get
            {
                string classe = "Chupeta";

                switch (Idade)
                {
                    case < 8:
                        classe = "Chupeta";
                        break;

                    case >= 8 and < 9:
                        classe = "Sub-9";
                        break;

                    case >= 9 and <= 10:
                        classe = "Sub-11";
                        break;

                    case >= 11 and <= 12:
                        classe = "Sub-13";
                        break;

                    case >= 13 and <= 14:
                        classe = "Sub-15";
                        break;

                    case >= 15 and <= 17:
                        classe = "Cadete";
                        break;

                    case >= 18 and <= 21:
                        classe = "Júnior";
                        break;

                    default:
                        classe = "Dangai";
                        break;
                }

                return classe;
            }



        }

        public required Sexo Sexo { get; set; } = Sexo.Masculino;

    }
    public enum Sexo
    {
        [Display(Name = "Fenimino")]
        [Description("Fenimino")]
        Feminino = 0,
        [Display(Name = "Masculino")]
        [Description("Masculino")]
        Masculino = 1,
        [Display(Name = "Prefiro não informar")]
        [Description("Prefiro não informar")]
        NaoInformar = 2
    }

    public enum TipoEscola
    {
        [Display(Name = "Privada")]
        [Description("Privada")]
        Privada = 0,
        [Display(Name = "Publica - Municipal")]
        [Description("Publica - Municipal")]
        Municipal = 1,
        [Display(Name = "Publica - Estadual")]
        [Description("Publica - Estadual")]
        Estadual = 2,
        [Display(Name = "Publica - Federal")]
        [Description("Publica - Federal")]
        Federal = 3
    }
}
