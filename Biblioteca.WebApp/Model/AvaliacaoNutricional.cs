using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class AvaliacaoNutricional : EntityBase
    {

        [Required]
        [ForeignKey(nameof(Atleta))]
        [Display(Name = "Atleta")]
        public int AtletaId { get; set; }
        
        [Display(Name = "Atleta")]
        public Atleta? Atleta { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime? Data { get; set; }


        [Range(typeof(decimal), "0", "3", ErrorMessage = "A altura não pode ser maior que 3 metros.")]
        [Column(TypeName = "decimal(3,2)")]
        [Display(Name = "Altura (m)")]
        public decimal? Altura { get; set; } = 0;

        [Required(ErrorMessage = "A Altura é obrigatória")]
        [Display(Name = "Altura (m)")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string AlturaAsString { get; set; }

        [Range(0, 200)]
        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "Peso (kg)")]
        public decimal? Peso { get; set; } = 0;

        [Required(ErrorMessage = "O Peso é obrigatório")]
        [Display(Name = "Peso (kg)")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string PesoAsString { get; set; }

        [StringLength(300)]
        [Display(Name = "Observação")]
        public string? Obs { get; set; }
        
        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "% de Gordura")]
        public decimal? PctGordura { get; set; } = 0;

        [Required(ErrorMessage = "% de Gordura é Obrigatório")]
        [Display(Name = "% de Gordura")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string PctGorduraAsString { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "% de Massa Muscular")]
        public decimal? MassaMuscular { get; set; } = 0;

        [Required(ErrorMessage = "% de Massa Muscular é Obrigatório")]
        [Display(Name = "% de Massa Muscular")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string MassaMuscularAsString { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "% de Massa Livre de Gordura")]
        public decimal? MassaLivre { get; set; } = 0;

        [Required(ErrorMessage = "% de Massa Livre de Gordura é Obrigatório")]
        [Display(Name = "% de Massa Livre de Gordura")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string MassaLivreAsString { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "Gordura Viceral")]
        public decimal? GorduraViceral { get; set; } = 0;

        [Required(ErrorMessage = "% Gordura Viceral é Obrigatório")]
        [Display(Name = "% Gordura Viceral")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string GorduraViceralAsString { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "Água Corporal")]
        public decimal? AguaCorporal { get; set; } = 0;

        [Required(ErrorMessage = "% Água Corporal é Obrigatório")]
        [Display(Name = "% Água Corporal")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string AguaCorporalAsString { get; set; }

        [ForeignKey(nameof(ArquivoImagem))]
        [Display(Name = "Imagem de Frente")]
        public int? ArquivoImagemId { get; set; }

        [Display(Name = "Imagem de Frente")]
        public Arquivo? ArquivoImagem { get; set; }

        [ForeignKey(nameof(ArquivoImagemCostas))]
        [Display(Name = "Imagem de Costas")]
        public int? ArquivoImagemCostasId { get; set; }

        [Display(Name = "Imagem de Costas")]
        public Arquivo? ArquivoImagemCostas { get; set; }

        public List<AvaliacaoNutricionalAnexo> Anexos { get; set; } = new();
    }

}
