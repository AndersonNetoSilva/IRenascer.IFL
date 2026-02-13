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
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IFL.WebApp.Areas.Admin.Pages.General.EstatisticasCompeticao
{
    public class EditModel : CrudPageModel<EstatisticaCompeticao, IEstatisticaCompeticaoRepository>
    {
        private readonly IAtletaRepository _atletaRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IEstatisticaCompeticaoService _estatisticaService;
        public int _eventoId = 0;
        public int _atletaId = 0;

        public EditModel(IEstatisticaCompeticaoRepository repository,
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

        [BindProperty]
        public EstatisticaCompeticao EstatisticaCompeticao { get; set; } = default!;

        [BindProperty]
        public List<EstatisticaCompeticaoDetalheVM> Detalhes { get; set; } = new();

        private void BindSelectLists()
        {
            Eventos = _eventoRepository.Query()
                        .Where(a => !a.Encerrado && a.TipoEvento == TipoEvento.Competicao)
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            BindSelectLists();

            if (id == null)
            {
                return NotFound();
            }

            //Método com o Include
            var estatistica = await _repository.GetForUpdateAsync(id);

            if (estatistica == null)
            {
                return NotFound();
            }

            _eventoId = estatistica.EventoId;
            _atletaId = estatistica.AtletaId;

            Detalhes = estatistica.Detalhes
                                .Select(p => new EstatisticaCompeticaoDetalheVM
                                {
                                    Id = p.Id,
                                    Ippon           = p.Ippon,
                                    Shido           = p.Shido,
                                    Yuko            = p.Yuko,
                                    Wazari          = p.Wazari,
                                    Hansokumake     = p.Hansokumake,
                                    Vitoria         = p.Vitoria ?? false,
                                    GoldenScore     = p.GoldenScore ?? false,
                                    TempoDaLuta     = p.TempoDaLuta,
                                    TempoDoGoldenScore      = p.TempoDoGoldenScore,
                                    TecnicaAplicou          = p.TecnicaAplicou,
                                    TecnicaRecebeu          = p.TecnicaRecebeu,
                                    EstatisticaCompeticaoId = p.EstatisticaCompeticaoId
                                })
                                .ToList();

            EstatisticaCompeticao = estatistica;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
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
                await _estatisticaService.UpdateAsync(EstatisticaCompeticao, Detalhes);
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
