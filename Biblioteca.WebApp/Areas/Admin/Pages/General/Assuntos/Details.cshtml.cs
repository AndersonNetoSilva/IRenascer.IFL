using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Assuntos
{
    public class DetailsModel : CrudPageModel<Assunto, IAssuntoRepository>
    {
        public DetailsModel(IAssuntoRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public Assunto Assunto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assunto = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (assunto == null)
            {
                return NotFound();
            }
            else
            {
                Assunto = assunto;
            }

            return Page();
        }
    }
}
