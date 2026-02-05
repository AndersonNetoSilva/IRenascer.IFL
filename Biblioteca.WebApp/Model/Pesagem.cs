using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class Pesagem : EntityBase
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

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Primeira Pesagem")]
        public DateTime? Pesagem1 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Segunda Pesagem")]
        public DateTime? Pesagem2 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Terceira Pesagem")]
        public DateTime? Pesagem3 { get; set; }


        [Range(0, 200)]
        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "Peso 1")]
        public decimal? Peso1 { get; set; } = 0;

        [Required(ErrorMessage = "O Peso 1 é obrigatório")]
        [Display(Name = "Peso 1")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string Peso1AsString { get; set; }

        [Display(Name = "Peso 2")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string? Peso2AsString { get; set; }

        [Display(Name = "Peso 3")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string? Peso3AsString { get; set; }


        [Range(0, 200)]
        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "Peso 2")]
        public decimal? Peso2 { get; set; } = 0;

        [Range(0, 200)]
        [Display(Name = "Peso 3")]
        [Column(TypeName = "decimal(6,2)")]
        public decimal? Peso3 { get; set; } = 0;

        [StringLength(200)]
        [Display(Name = "Observação")]
        public string? Obs1 { get; set; }

        [StringLength(200)]
        [Display(Name = "Observação")]
        public string? Obs2 { get; set; }

        [StringLength(200)]
        [Display(Name = "Observação")]
        public string? Obs3 { get; set; }

        [StringLength(60)]
        [Display(Name = "Categoria")]
        public string? Categoria1 { get; set; }

        [StringLength(60)]
        [Display(Name = "Categoria")]
        public string? Categoria2 { get; set; }

        [StringLength(60)]
        [Display(Name = "Categoria")]
        public string? Categoria3 { get; set; }

        [StringLength(60)]
        [Display(Name = "Categoria")]
        public string? Categoria4 { get; set; }

    }
     
}
