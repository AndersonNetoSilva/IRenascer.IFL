using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Modalidades
{
    public class DetailsModel : CrudPageModel<Modalidade, IModalidadeRepository>
    {
        public DetailsModel(IModalidadeRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public Modalidade Modalidade { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modalidade = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (modalidade == null)
            {
                return NotFound();
            }
            else
            {
                Modalidade = modalidade;
            }
            return Page();
        }
    }
}
