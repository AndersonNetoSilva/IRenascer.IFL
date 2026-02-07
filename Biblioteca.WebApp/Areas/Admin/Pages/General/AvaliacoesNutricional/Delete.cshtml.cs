using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace IFL.WebApp.Areas.Admin.Pages.General.AvaliacoesNutricional
{
    public class DeleteModel : CrudPageModel<AvaliacaoNutricional, IAvaliacaoNutricionalRepository>
    {

        public DeleteModel(IAvaliacaoNutricionalRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
           
        }

        [BindProperty]
        public AvaliacaoNutricional AvaliacaoNutricional { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoNutricional = await _repository.GetForUpdateAsync(id ?? throw new ArgumentException(nameof(id)));

            if (avaliacaoNutricional == null)
            {
                return NotFound();
            }
            else
            {
                AvaliacaoNutricional = avaliacaoNutricional;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoNutricional = await _repository.GetForUpdateAsync(id ?? throw new ArgumentException(nameof(id)));

            if (avaliacaoNutricional != null)
            {
                AvaliacaoNutricional = avaliacaoNutricional;

                _repository.Remove(AvaliacaoNutricional);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
