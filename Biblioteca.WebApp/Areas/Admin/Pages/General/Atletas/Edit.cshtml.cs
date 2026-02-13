using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Infrastructure.Services;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.Atletas
{
    public class EditModel : CrudPageModel<Atleta, IAtletaRepository>
    {
        private readonly IModalidadeRepository _modalidadeRepository;
        private readonly IHorarioRepository _horarioRepository;
        private readonly IAtletaService _atletaService;

        public EditModel(IAtletaRepository repository,
                         IModalidadeRepository modalidadeRepository,
                         IHorarioRepository horarioRepository,
                         IAtletaService atletaService,
                         IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _modalidadeRepository = modalidadeRepository;
            _horarioRepository = horarioRepository;
            _atletaService = atletaService;

        }

        [BindProperty]
        public Atleta Atleta { get; set; } = default!;

        [BindProperty]
        public List<AtletaGradeVM> AtletaGrades { get; set; } = new();
        public List<SelectListItem> Horarios { get; set; } = new();
        public List<SelectListItem> Modalidades { get; set; } = new();

        [BindProperty]
        public ArquivoVM ArquivoImagem { get; set; } = new();
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
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atleta = await _repository.GetForUpdateAsync(id);

            if (atleta == null)
            {
                return NotFound();
            }

            AtletaGrades = atleta.AtletaGrades
                                .Select(p => new AtletaGradeVM
                                {
                                    Id = p.Id,
                                    ModalidadeId = p.ModalidadeId,
                                    HorarioId = p.HorarioId
                                })
                                .ToList();

            Atleta = atleta;

            BindSelectLists();

            return Page();
        }

        public async Task<IActionResult> OnGetConteudoDoArquivoAsync(int atletaId)
        {
            var atleta = await _repository.Query()
                                .Include(x => x.ArquivoImagem)
                                .FirstOrDefaultAsync(x => x.Id == atletaId);

            if (atleta?.ArquivoImagem == null)
                return NotFound();

            return File(
                atleta.ArquivoImagem.Conteudo,
                atleta.ArquivoImagem.ContentType
            );
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
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
                await _atletaService.UpdateAsync(Atleta, AtletaGrades, ArquivoImagem);
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
