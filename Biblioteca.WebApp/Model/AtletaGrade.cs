using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class AtletaGrade : EntityBase
    {
        [Display(Name = "Atleta")]
        public Atleta? Atleta { get; set; }

        [Required]
        [ForeignKey(nameof(Atleta))]
        [Display(Name = "Atleta")]
        public int AtletaId { get; set; }

        [Required]
        [ForeignKey(nameof(Horario))]
        [Display(Name = "Horario")]
        public int HorarioId { get; set; }

        [Required]
        [ForeignKey(nameof(Modalidade))]
        [Display(Name = "Modalidade")]
        public int ModalidadeId { get; set; }
        
        [Display(Name = "Horario")]
        public Horario? Horario { get; set; }
        
        [Display(Name = "Modalidade")]
        public Modalidade? Modalidade { get; set; }

    }

    public class AtletaGradeVM : IPermiteMarcarParaExclusao
    {
        public int? Id { get; set; }
        public TipoDeVenda Tipo { get; set; }
        public int ModalidadeId { get; set; }
        public int HorarioId { get; set; }
        public int AtletaId { get; set; }
        public bool MarcadoParaExclusao { get; set; } = false;
    }

    public interface IPermiteMarcarParaExclusao
    {
        bool MarcadoParaExclusao { get; set; }
    }

}
