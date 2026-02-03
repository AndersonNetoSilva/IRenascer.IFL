using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Colaboradores
{
    public class DeleteModel : CrudPageModel<Colaborador, IColaboradorRepository>
    {
        public DeleteModel(IColaboradorRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
        public Colaborador Colaborador { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (colaborador == null)
            {
                return NotFound();
            }
            else
            {
                Colaborador = colaborador;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (colaborador != null)
            {
                Colaborador = colaborador;

                _repository.Remove(Colaborador);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
