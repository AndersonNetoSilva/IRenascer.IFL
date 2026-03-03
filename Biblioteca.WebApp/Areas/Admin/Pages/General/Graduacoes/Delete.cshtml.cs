using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Graduacoes
{
    public class DeleteModel : CrudPageModel<Graduacao, IGraduacaoRepository>
    {
        public DeleteModel(IGraduacaoRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
        public Graduacao Graduacao { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graduacao = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (graduacao == null)
            {
                return NotFound();
            }
            else
            {
                Graduacao = graduacao;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graduacao = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (graduacao != null)
            {
                Graduacao = graduacao;

                _repository.Remove(Graduacao);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
