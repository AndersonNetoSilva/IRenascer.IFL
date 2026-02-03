using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Horarios
{
    public class EditModel : CrudPageModel<Horario, IHorarioRepository>
    {
        public EditModel(IHorarioRepository repository, IUnitOfWork unitOfWork)
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

            Horario = horario;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _repository.Update(Horario);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.ExistsAsync(x => x.Id == Horario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
