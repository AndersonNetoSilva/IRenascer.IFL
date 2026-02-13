using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IFL.WebApp.Areas.Admin.Pages.General.Modalidades
{
    public class DeleteModel : CrudPageModel<Modalidade, IModalidadeRepository>
    {
        private readonly IAtletaRepository _atletaRepository;
        public DeleteModel(IModalidadeRepository repository, IUnitOfWork unitOfWork,
                                IAtletaRepository atletaRepository)
            : base(repository, unitOfWork)
        {
            _atletaRepository = atletaRepository;       
        }

        [BindProperty]
        public Modalidade Modalidade { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modalidade = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (modalidade == null)
            {
                return NotFound();
            }
            else
            {
                Modalidade = modalidade;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modalidade = await _repository.GetByIdAsync(id ?? throw new ArgumentException(nameof(id)));

            if (modalidade != null)
            {
                Modalidade = modalidade;

                try
                {
                    if (_atletaRepository.ExiteAtletaNaModalidade(id))
                        throw new Exception("Existe Modalidade associada a Atleta.");

                    _repository.Remove(Modalidade);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return Page();
                }

            }

            return RedirectToPage("./Index");
        }
    }
}
