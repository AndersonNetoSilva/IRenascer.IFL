using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.AvaliacoesNutricional
{
    public class DetailsModel : CrudPageModel<AvaliacaoNutricional, IAvaliacaoNutricionalRepository>
    {
        public DetailsModel(IAvaliacaoNutricionalRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public AvaliacaoNutricional AvaliacaoNutricional { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoNutricional = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

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
    }
}
