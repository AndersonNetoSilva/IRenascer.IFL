using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class EstatisticaCompeticao : EntityBase
    {
        [Required]
        [ForeignKey(nameof(Evento))]
        [Display(Name = "Evento")]
        public int EventoId { get; set; }

        [Required]
        [ForeignKey(nameof(Atleta))]
        [Display(Name = "Atleta")]
        public int AtletaId { get; set; }
        
        [Display(Name = "Evento")]
        public Evento? Evento { get; set; }
        
        [Display(Name = "Atleta")]
        public Atleta? Atleta { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime? Data { get; set; }

        public List<EstatisticaCompeticaoDetalhe> Detalhes { get; set; } = new();
    }
     
}
