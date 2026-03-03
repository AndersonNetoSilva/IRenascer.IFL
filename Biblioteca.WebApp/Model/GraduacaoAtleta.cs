using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class GraduacaoAtleta : EntityBase
    {
        [Required]
        [ForeignKey(nameof(Graducao))]
        [Display(Name = "Graducao")]
        public int GraducaoId { get; set; }

        [Display(Name = "Graducao")]
        public Graduacao? Graducao { get; set; }

        [Display(Name = "Aprovado")]
        public bool? Aprovado { get; set; }

        [Required]
        [ForeignKey(nameof(Atleta))]
        [Display(Name = "Atleta")]
        public int AtletaId { get; set; }

        [Display(Name = "Atleta")]
        public Atleta? Atleta { get; set; }

        [Range(0, 10)]
        [Column(TypeName = "decimal(4,2)")]
        [Display(Name = "Nota Escrita")]
        public decimal? NotaEscrita { get; set; }

        [Range(0, 10)]
        [Column(TypeName = "decimal(4,2)")]
        [Display(Name = "Nota Prática")]
        public decimal? NotaPratica { get; set; }

        [Display(Name = "Faixa Atual")]
        public string? FaixaAtual
        {
            get
            {
                return Atleta?.GraduacaoAsString; 
            }
        }

        [Display(Name = "Faixa Nova")]
        public required GraduacaoJudo FaixaNova { get; set; }

        [ForeignKey(nameof(Arquivo))]
        [Display(Name = "Anexo (Examo)")]
        public int? ArquivoId { get; set; }

        [Display(Name = "Anexo (Exame)")]
        public Arquivo? Arquivo { get; set; }

        [Display(Name = "Tipo de Anexo")]
        public TipoANexo? Tipo { get; set; }
    }

    public class GraduacaoAtletaVM : IPermiteMarcarParaExclusao
    {
        [Display(Name = "#")]
        public int? Id { get; set; }
        public int Index { get; set; } = 0;
        public int GraduacaoId { get; set; } = 0;
        public bool Aprovado { get; set; } = false;
        public int AtletaId { get; set; }
        public string? Atleta { get; set; }

        [Display(Name = "Escrita ")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string? NotaEscritaAsString { get; set; }

        [Display(Name = "Escrita")]
        public decimal? NotaEscrita { get; set; }

        [Display(Name = "Prática ")]
        [NotMapped]
        [RegularExpression(@"^\d+(,\d{2})$", ErrorMessage = "Informe um valor com exatamente 2 casas decimais.")]
        public string? NotaPraticaAsString { get; set; }

        [Display(Name = "Prática")]
        public decimal? NotaPratica { get; set; }

        [Display(Name = "Faixa atual")]
        public string? FaixaAtual { get; set; }
        
        [Display(Name = "Faixa Nova")]
        public required GraduacaoJudo FaixaNova { get; set; }
        public IFormFile? FormFile { get; set; }
        public string? Tipo { get; set; }
        public bool MarcadoParaExclusao { get; set; } = false;
    }



}
