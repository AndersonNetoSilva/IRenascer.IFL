using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace IFL.WebApp.Model
{
    public class Modalidade : EntityBase
    {
        [Required]
        [StringLength(60)]
        [Display(Name = "Nome")]
        public required string Nome { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }
         
        [Display(Name = "Modalidade Ativa")]
        public bool Ativo { get; set; } = true;

    }

}
