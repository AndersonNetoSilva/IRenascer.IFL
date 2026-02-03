using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace IFL.WebApp.Areas.Admin.Pages.General.Horarios
{
    public class CreateModel : CrudPageModel<Horario, IHorarioRepository>
    {
        public CreateModel(IHorarioRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Horario Horario { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _repository.Add(Horario);
            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
