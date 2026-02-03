using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Pesagens
{
    public class DetailsModel : CrudPageModel<Pesagem, IPesagemRepository>
    {
        public DetailsModel(IPesagemRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public Pesagem Pesagem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pesagem = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (pesagem == null)
            {
                return NotFound();
            }
            else
            {
                Pesagem = pesagem;
            }
            return Page();
        }
    }
}
