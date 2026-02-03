using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Atletas
{
    public class DetailsModel : CrudPageModel<Atleta, IAtletaRepository>
    {
        public DetailsModel(IAtletaRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public Atleta Atleta { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atleta = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (atleta == null)
            {
                return NotFound();
            }
            else
            {
                Atleta = atleta;
            }
            return Page();
        }
    }
}
