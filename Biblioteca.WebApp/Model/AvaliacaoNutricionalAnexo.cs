using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class AvaliacaoNutricionalAnexo : EntityBase
    {
        [Required]
        [StringLength(40)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        [Display(Name = "Tipo de Anexo")]
        public TipoANexo? Tipo { get; set; }

        [Display(Name = "Avaliação Nutricional")]
        public AvaliacaoNutricional? AvaliacaoNutricional { get; set; }

        [Required]
        [ForeignKey(nameof(AvaliacaoNutricional))]
        [Display(Name = "Avaliação Nutricional")]
        public int AvaliacaoNutricionalId { get; set; }

        [ForeignKey(nameof(Anexo))]
        [Display(Name = "Imagem")]
        public int? AnexoId { get; set; }

        [Display(Name = "Imagem")]
        public Arquivo? Anexo { get; set; }
    }

    public class AvaliacaoNutricionalAnexoVM : IPermiteMarcarParaExclusao
    {
        public int? Id { get; set; }
        public string? Descricao { get; set; }
        public IFormFile? FormFile { get; set; }
        public bool MarcadoParaExclusao { get; set; } = false;
        public string? Tipo { get; set; }
    }

    public enum TipoANexo
    {
        Imagem,
        Arquivo
    }

}
