using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace IFL.WebApp.Areas.Admin.Pages.General.Eventos
{
    public class CreateModel : CrudPageModel<Evento, IEventoRepository>
    {
        public CreateModel(IEventoRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Evento Evento { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _repository.Add(Evento);
            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
