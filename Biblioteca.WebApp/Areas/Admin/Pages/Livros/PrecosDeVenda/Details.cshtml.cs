using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.PrecosDeVenda
{
    public class DetailsModel : CrudPageModel<PrecoDeVenda, IPrecoDeVendaRepository>
    {
        public DetailsModel(IPrecoDeVendaRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        public PrecoDeVenda PrecoDeVenda { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _repository
                                .Query()
                                .Where(x => x.Id == id.Value)
                                .Include(x => x.Livro)
                                .FirstOrDefaultAsync();

            if (livro == null)
            {
                return NotFound();
            }
            else
            {
                PrecoDeVenda = livro;
            }

            return Page();
        }
    }
}
