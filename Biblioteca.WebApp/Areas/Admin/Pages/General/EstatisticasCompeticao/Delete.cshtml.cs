using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.EstatisticasCompeticao
{
    public class DeleteModel : CrudPageModel<EstatisticaCompeticao, IEstatisticaCompeticaoRepository>
    {
        public DeleteModel(IEstatisticaCompeticaoRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
        public EstatisticaCompeticao EstatisticaCompeticao { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatistica = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (estatistica == null)
            {
                return NotFound();
            }
            else
            {
                EstatisticaCompeticao = estatistica;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatistica = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (estatistica != null)
            {
                EstatisticaCompeticao = estatistica;

                _repository.Remove(EstatisticaCompeticao);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
