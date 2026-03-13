using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Services;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.WebEncoders.Testing;
using IFL.WebApp.Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace IFL.WebApp.Areas.Admin.Pages.General.Graduacoes
{
    public class CreateModel :   PageModel
    {
        private readonly IModalidadeRepository _modalidadeRepository;
        private readonly IHorarioRepository _horarioRepository;
        private readonly IAtletaRepository _atletaRepository;
        private readonly IGraduacaoService _graduacaoService;

        public CreateModel(IGraduacaoRepository repository,
                           IAtletaRepository atletaRepository,
                           IModalidadeRepository modalidadeRepository,
                           IHorarioRepository horarioRepository,
                           IGraduacaoService graduacaoService,
                           IUnitOfWork unitOfWork)
            : base()
        {
            _modalidadeRepository = modalidadeRepository;
            _horarioRepository = horarioRepository;
            _atletaRepository = atletaRepository;
            _graduacaoService = graduacaoService;
        }

        public List<SelectListItem> Horarios { get; set; } = new();
        public List<SelectListItem> Modalidades { get; set; } = new();

        public IActionResult OnGet()
        {
            GraduacaoVM = new GraduacaoVM();

            BindSelectLists();
            return Page();
        }

        //[BindProperty]
        //public Graduacao Graduacao { get; set; } = default!;

        [BindProperty]
        public GraduacaoVM GraduacaoVM { get; set; } = default!;

        [BindProperty]
        public List<GraduacaoAtletaVM> GraduacaoAtletaVM { get; set; } = new();

        public async Task<IActionResult> OnGetFillRegistrosGraduacaoAtletaAsync( int? idGraduacao = null,
                                                                           int? idHorario = null,
                                                                           int? idModalidade = null)
        {
            GraduacaoVM = await FiltrarRegistrosAsync(idGraduacao,idHorario, idModalidade);

            GraduacaoAtletaVM = GraduacaoVM.GraduacaoAtletasVM;

            return Partial("_RegistrosAtletaGraduacaoGrid", GraduacaoVM);
        }

        public async Task<IActionResult> OnGetNewRegistroGraduacaoAtletaAsync(int index, int graduacaoId)
        {
            var graduacaoAtleta = new GraduacaoAtletaVM
            {
                Index = index,
                FaixaNova = GraduacaoJudo.FaixaBranca,
                GraduacaoId = graduacaoId
            };


            return Partial("_RegistrosAtletaGraduacaoGridRow", graduacaoAtleta);
        }

        private async Task<GraduacaoVM> FiltrarRegistrosAsync(int? idGraduacao, int? idHorario, int? idModalidade)
        {
            var graduacao = new GraduacaoVM()
            {
                Descricao = string.Empty,
                Data = DateTime.Now,
                Id = 0
            };

            var atletas = _atletaRepository
                            .Query()
                            .Where(x => x.AtletaGrades
                                            .Any(y => y.ModalidadeId == idModalidade &&
                                                      y.HorarioId == idHorario)
                            )
                            .OrderBy(x=>x.Nome)
                            .ToList();

            if (atletas.Any())
            {
                int i = 1;
                foreach (var item in atletas)
                {
                    // 1. Obtém todas as faixas configuradas no seu Enum
                    var valoresEnum = Enum.GetValues(item.Graduacao.GetType());

                    // 2. Acha a posição (índice) da faixa atual do atleta na lista
                    int indexAtual = Array.IndexOf(valoresEnum, item.Graduacao);

                    // 3. Define a próxima faixa. Se ele já estiver na última, mantém a mesma.
                    var proximaFaixa = (indexAtual >= 0 && indexAtual < valoresEnum.Length - 1)
                                       ? valoresEnum.GetValue(indexAtual + 1)
                                       : item.Graduacao;

                    graduacao.GraduacaoAtletasVM.Add(new GraduacaoAtletaVM()
                    {
                        Id = (i + 1),
                        Atleta = item.Nome,
                        AtletaId = item.Id,
                        FaixaNova = (IFL.WebApp.Model.GraduacaoJudo)proximaFaixa,
                        FaixaAtual = item.GraduacaoAsString,
                        Aprovado = false,
                        NotaEscrita = 0.00m,
                        NotaPratica = 0.00m
                    });

                    i++;
                }                
            }

            return graduacao;
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
                // A sua validação de regra de negócio customizada
                if (!ValidarGraduacao(GraduacaoVM))
                {
                    BindSelectLists();
                    return Page(); // Retorna para a tela; o ModelState já contém a sua mensagem de erro.
                }

                ValidarValor(GraduacaoVM);
                Graduacao graduacao = SetGraduacao(GraduacaoVM);

                await _graduacaoService.AddAsync(graduacao, GraduacaoVM.GraduacaoAtletasVM);
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

        private bool ValidarGraduacao(GraduacaoVM graduacaoVM)
        {
            if (_graduacaoService.ExisteGraduacaoNoPeriodo(graduacaoVM))
            {
                ModelState.AddModelError(string.Empty, "Existe Graduação para esse horário cadastrado.");
                return false;
            }

            return true;
        }

        public static void ValidarValor(GraduacaoVM graduacaoVM)
        {
            foreach (var item in graduacaoVM.GraduacaoAtletasVM)
            {
                if (item.NotaEscritaAsString.IsNullOrEmpty())
                    item.NotaEscritaAsString = "00,00";

                if (item.NotaPraticaAsString.IsNullOrEmpty())
                    item.NotaPraticaAsString = "00,00";

                if (!item.NotaEscritaAsString.TryParseValor(out string errorMessage, out var valorDecimal))
                {
                    throw new Infrastructure.Exceptions.ValidationListException(("item.NotaEscritaAsString", errorMessage));
                }

                item.NotaEscrita = valorDecimal;

                if (!item.NotaPraticaAsString.TryParseValor(out string errorMessage2, out  valorDecimal))
                {
                    throw new Infrastructure.Exceptions.ValidationListException(("item.NotaPraticaAsString", errorMessage2));
                }

                item.NotaPratica = valorDecimal;
            }
        }

        private Graduacao SetGraduacao(GraduacaoVM graduacaoVM)
        {
            Graduacao graduacao = new Graduacao
                                        {
                                            Descricao   = graduacaoVM.Descricao,
                                            Data        = graduacaoVM.Data.Value,
                                            HorarioId   = graduacaoVM.HorarioId,
                                            Horario     = _horarioRepository.Query().Where(x=> x.Id == graduacaoVM.HorarioId).FirstOrDefault(),
                                            ModalidadeId = graduacaoVM.ModalidadeId,
                                            Modalidade  = _modalidadeRepository.Query().Where(x => x.Id == graduacaoVM.ModalidadeId).FirstOrDefault(),
                                            Encerrada   = graduacaoVM.Encerrada,
                                            Id          =graduacaoVM.Id
                                        };

            foreach (var item in graduacaoVM.GraduacaoAtletasVM)
            {
                GraduacaoAtleta g = new GraduacaoAtleta { FaixaNova = item.FaixaNova,
                                                          AtletaId = item.AtletaId,
                                                          Atleta = _atletaRepository.Query().Where(x => x.Id == item.AtletaId).FirstOrDefault(),
                                                          Aprovado = item.Aprovado,
                                                          GraducaoId = item.GraduacaoId,
                                                          NotaEscrita = item.NotaEscrita,
                                                          NotaPratica = item.NotaPratica};
                graduacao.GraduacaoAtletas.Add(g);
            }

            return graduacao;
        }
    }
}
