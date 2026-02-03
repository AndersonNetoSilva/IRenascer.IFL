using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace IFL.WebApp.Areas.Admin.Pages.General.Modalidades
{
    public class CreateModel : CrudPageModel<Modalidade, IModalidadeRepository>
    {
        public CreateModel(IModalidadeRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Modalidade Modalidade { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _repository.Add(Modalidade);
            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
