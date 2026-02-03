using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model
{
    public class Autor : EntityBase
    {
        [Required]
        [StringLength(40)]
        [Display(Name = "Nome")]
        public required string Nome { get; set; }

        public List<Livro> Livros { get; set; } = new();
    }
}
