using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Horarios
{
    public class DetailsModel : CrudPageModel<Horario, IHorarioRepository>
    {
        public DetailsModel(IHorarioRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }

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
    }
}
