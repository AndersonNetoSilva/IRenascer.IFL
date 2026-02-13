using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class AnexoBase : EntityBase
    {

        [Required]
        [StringLength(200)]
        [Display(Name = "Descrição")]
        public required string DescricaoImagem { get; set; }

        [ForeignKey(nameof(ArquivoImagem))]
        [Display(Name = "Imagem")]
        public int? ArquivoImagemId { get; set; }

        [Display(Name = "Imagem")]
        public Arquivo? ArquivoImagem { get; set; }

    }

    public class AnexoVM
    {
        public int Id { get; set; }
        public string? Descricao { get; set; } = null;
        public IFormFile? FormFileImagem { get; set; }

    }

}
