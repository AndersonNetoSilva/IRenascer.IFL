using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class Evento : EntityBase
    {
        [Required]
        [StringLength(60)]
        [Display(Name = "Nome")]
        public required string Nome { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "Local")]
        public required string Local { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public required DateTime Data { get; set; } = DateTime.Now;

        [Display(Name = "Tipo do Evento")]
        public required TipoEvento? TipoEvento { get; set; }

        [Display(Name = "Evento encerrado")]
        public bool Encerrado { get; set; } = false;
    }


    public enum TipoEvento
    {
        [Display(Name = "Evento Local")]
        [Description("Evento Local")]
        Evento = 0,
        [Display(Name = "Competição")]
        [Description("Competição")]
        Competicao = 1,
        [Display(Name = "Evento Externo")]
        [Description("Evento Externo")]
        EventoExterno = 2
    }

}
