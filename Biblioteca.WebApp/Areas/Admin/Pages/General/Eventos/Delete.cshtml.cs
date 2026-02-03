using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Eventos
{
    public class DeleteModel : CrudPageModel<Evento, IEventoRepository>
    {
        public DeleteModel(IEventoRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (evento != null)
            {
                Evento = evento;

                _repository.Remove(Evento);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
