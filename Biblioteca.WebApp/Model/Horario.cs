using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model
{
    public class Horario : EntityBase
    {
        [Required]
        [StringLength(60)]
        [Display(Name = "Nome")]
        public required string Nome { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }
        public DiaDaSemana DiaSemana { get; set; } = DiaDaSemana.Segunda;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }

        [Display(Name = "Domingo")]
        public bool Domingo { get; set; } = false;

        [Display(Name = "Segunda-feira")]
        public bool Segunda { get; set; } = false;

        [Display(Name = "Terça-feira")]
        public bool Terca { get; set; } = false;
        [Display(Name = "Quarta-feira")]
        public bool Quarta { get; set; } = false;

        [Display(Name = "Quinta-feira")]
        public bool Quinta { get; set; } = false;

        [Display(Name = "Sexta-feira")]
        public bool Sexta { get; set; } = false;

        [Display(Name = "Sábado")]
        public bool Sabado { get; set; } = false;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }

    public enum DiaDaSemana
    {
        [Display(Name = "Domingo")]
        [Description("Domingo")]
        Domingo = 0,
        [Display(Name = "Segunda-feira")]
        [Description("Segunda")]
        Segunda = 1,
        [Display(Name = "Terça-feira")]
        [Description("Terca")]
        Terca = 2,
        [Display(Name = "Quarta-feira")]
        [Description("Quarta")]
        Quarta = 3,
        [Display(Name = "Quinta-feira")]
        [Description("Quinta")]
        Quinta = 4,
        [Display(Name = "Sexta-feira")]
        [Description("Sexta")]
        Sexta = 5,
        [Display(Name = "Sábado")]
        [Description("Sabado")]
        Sabado = 6
    }
}
