using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Extensions;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Areas.Admin.Pages.General.EstatisticasCompeticao
{
    public class CreateModel : CrudPageModel<EstatisticaCompeticao, IEstatisticaCompeticaoRepository>
    {
        private readonly IAtletaRepository _atletaRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IEstatisticaCompeticaoService _estatisticaService;

        public CreateModel(IEstatisticaCompeticaoRepository repository,
                           IAtletaRepository atletaRepository,
                           IEventoRepository eventoRepository,
                           IEstatisticaCompeticaoService estatisticaService,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _atletaRepository = atletaRepository;
            _eventoRepository = eventoRepository;
            _estatisticaService = estatisticaService;
        }

        public IActionResult OnGet()
        {
            BindSelectLists();
            return Page();
        }

        private void BindSelectLists()
        {
            Eventos = _eventoRepository.Query()
                        .Where(a=> !a.Encerrado && a.TipoEvento == TipoEvento.Competicao)
                        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                        .OrderBy(a => a.Text)
                        .ToList();

            Atletas = _atletaRepository.Query()
                            .Where(a => a.Ativo)
                            .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                            .OrderBy(a => a.Text)
                            .ToList();
        }
        public List<SelectListItem> Eventos { get; set; } = new();
        public List<SelectListItem> Atletas { get; set; } = new();

        public List<SelectListItem> Tecnicas { get; set; } = new();

        [BindProperty]
        public EstatisticaCompeticao EstatisticaCompeticao { get; set; } = default!;

        [BindProperty]
        public List<EstatisticaCompeticaoDetalheVM> Detalhes { get; set; } = new();

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            EstatisticaCompeticao.Atleta = _atletaRepository.Query().Where(a => EstatisticaCompeticao.AtletaId == a.Id).FirstOrDefault();
            EstatisticaCompeticao.Evento = _eventoRepository.Query().Where(a => EstatisticaCompeticao.EventoId == a.Id).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                BindSelectLists();
                return Page();
            }

            try
            {
                await _estatisticaService.AddAsync(EstatisticaCompeticao, Detalhes);
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
