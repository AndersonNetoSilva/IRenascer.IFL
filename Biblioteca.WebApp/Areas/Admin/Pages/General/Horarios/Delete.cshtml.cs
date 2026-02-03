using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Horarios
{
    public class DeleteModel : CrudPageModel<Horario, IHorarioRepository>
    {
        public DeleteModel(IHorarioRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

        [BindProperty]
        public Horario Horario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (horario == null)
            {
                return NotFound();
            }
            else
            { 
                Horario = horario;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (horario != null)
            {
                Horario = horario;

                _repository.Remove(Horario);
                await _unitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
