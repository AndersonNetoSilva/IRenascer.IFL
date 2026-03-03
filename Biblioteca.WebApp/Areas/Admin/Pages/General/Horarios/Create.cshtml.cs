using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IFL.WebApp.Areas.Admin.Pages.General.Horarios
{
    public class CreateModel : CrudPageModel<Horario, IHorarioRepository>
    {
        private readonly IModalidadeRepository _modalidadeRepository;
        public CreateModel(IHorarioRepository repository, IUnitOfWork unitOfWork,
            IModalidadeRepository modalidadeRepository)
            : base(repository, unitOfWork)
        {
            _modalidadeRepository= modalidadeRepository;
        }

        public IActionResult OnGet()
        {
            BindSelectLists();
            return Page();
        }

        private void BindSelectLists()
        {
            Modalidades = _modalidadeRepository.Query()
                            .Where(a => a.Ativo)
                            .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                            .OrderBy(a => a.Text)
                            .ToList();
        }
        public List<SelectListItem> Modalidades { get; set; } = new();

        [BindProperty]
        public Horario Horario { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            
            _repository.Add(Horario);
            await _unitOfWork.CommitAsync();

            return RedirectToPage("./Index");
        }
    }
}
