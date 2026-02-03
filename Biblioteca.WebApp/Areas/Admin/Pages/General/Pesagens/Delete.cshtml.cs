using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Pesagens
{
    public class DeleteModel : CrudPageModel<Pesagem, IPesagemRepository>
    {
        public DeleteModel(IPesagemRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pesagem = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (pesagem != null)
            {
                Pesagem = pesagem;

                _repository.Remove(Pesagem);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
