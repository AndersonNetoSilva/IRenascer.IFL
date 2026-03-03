using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class Graduacao : EntityBase
    {
        [Required]
        [ForeignKey(nameof(Horario))]
        [Display(Name = "Horario")]
        public int HorarioId { get; set; }

        [Display(Name = "Horario")]
        public Horario? Horario { get; set; }

        [Required]
        [ForeignKey(nameof(Modalidade))]
        [Display(Name = "Modalidade")]
        public int ModalidadeId { get; set; }        
        
        [Display(Name = "Modalidade")]
        public Modalidade? Modalidade { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        [Display(Name = "Encerrada")]
        public bool Encerrada { get; set; } = false;

        public List<GraduacaoAtleta> GraduacaoAtletas { get; set; } = new();
    }


    public class GraduacaoVM
    {
        public int Id { get; set; } = 0;

        [Display(Name = "Horário")]
        public int HorarioId { get; set; }

        [Display(Name = "Modalidade")]
        public int ModalidadeId { get; set; }
        public DateTime? Data { get; set; }
        public string? Descricao { get; set; }
        public bool Encerrada { get; set; } = false;

        public List<GraduacaoAtletaVM> GraduacaoAtletasVM { get; set; } = new();
    }

}
