using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Eventos
{
    public class DetailsModel : CrudPageModel<Evento, IEventoRepository>
    {
        public DetailsModel(IEventoRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public Evento Evento { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (evento == null)
            {
                return NotFound();
            }
            else
            {
                Evento = evento;
            }

            return Page();
        }
    }
}
