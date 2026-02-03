using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Autores
{
    public class DetailsModel : CrudPageModel<Autor, IAutorRepository>
    {
        public DetailsModel(IAutorRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public Autor Autor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (autor == null)
            {
                return NotFound();
            }
            else
            {
                Autor = autor;
            }
            return Page();
        }
    }
}
