using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Migrations;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IFL.WebApp.Areas.Admin.Pages.General.Atletas
{
    public class CreateModel : CrudPageModel<Atleta, IAtletaRepository>
    {
        private readonly IModalidadeRepository _modalidadeRepository;
        private readonly IHorarioRepository _horarioRepository;
        private readonly IAtletaService _atletaService;

        public CreateModel(IAtletaRepository repository,
                           IModalidadeRepository modalidadeRepository,
                           IHorarioRepository    horarioRepository,
                           IAtletaService atletaService,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _modalidadeRepository = modalidadeRepository;
            _horarioRepository=horarioRepository;
            _atletaService = atletaService; 
        }

        public IActionResult OnGet()
        {          
            BindSelectLists();
            return Page();
        }
        private void BindSelectLists()
        {
            Horarios = _horarioRepository.Query()
                           .Where(a => a.Ativo)
                           .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                           .OrderBy(a => a.Text)
                           .ToList();

            Modalidades = _modalidadeRepository.Query()
                           .Where(a => a.Ativo)
                           .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                           .OrderBy(a => a.Text)
                           .ToList();
        }

        [BindProperty]
        public Atleta Atleta { get; set; } = default!;

        [BindProperty]
        public List<AtletaGradeVM> AtletaGrades { get; set; } = new();

        public List<SelectListItem> Horarios { get; set; } = new();
        public List<SelectListItem> Modalidades { get; set; } = new();
        
        [BindProperty]
        public ArquivoVM ArquivoImagem { get; set; } = new();

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                BindSelectLists();
                return Page();
            }

            try
            {
                await _atletaService.AddAsync(Atleta, AtletaGrades ,ArquivoImagem);
            }
            catch (KeyNotFoundException)
            {
                BindSelectLists();
                return NotFound();
            }
            catch (ValidationListException ex)
            {
                BindSelectLists();

                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.Key, error.Value);

                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
