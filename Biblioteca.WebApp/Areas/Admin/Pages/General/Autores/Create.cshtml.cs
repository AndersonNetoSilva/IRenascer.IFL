using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace IFL.WebApp.Areas.Admin.Pages.General.Autores
{
    public class CreateModel : CrudPageModel<Autor, IAutorRepository>
    {
        public CreateModel(IAutorRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Autor Autor { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _repository.Add(Autor);
            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
