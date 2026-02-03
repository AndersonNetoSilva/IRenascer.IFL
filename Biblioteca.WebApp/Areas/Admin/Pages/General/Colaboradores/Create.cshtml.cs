using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace IFL.WebApp.Areas.Admin.Pages.General.Colaboradores
{
    public class CreateModel : CrudPageModel<Colaborador, IColaboradorRepository>
    {
        public CreateModel(IColaboradorRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Colaborador Colaborador { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _repository.Add(Colaborador);
            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
