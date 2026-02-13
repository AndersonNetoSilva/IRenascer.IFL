using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.EstatisticasCompeticao
{
    public class DetailsModel : CrudPageModel<EstatisticaCompeticao, IEstatisticaCompeticaoRepository>
    {
        public DetailsModel(IEstatisticaCompeticaoRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public EstatisticaCompeticao EstatisticaCompeticao { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatisticaCompeticao = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (estatisticaCompeticao == null)
            {
                return NotFound();
            }
            else
            {
                EstatisticaCompeticao = estatisticaCompeticao;
            }
            return Page();
        }
    }
}
