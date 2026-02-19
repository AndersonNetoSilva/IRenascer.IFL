using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.General.AvaliacoesNutricional
{
    public class EditModel : CrudPageModel<AvaliacaoNutricional, IAvaliacaoNutricionalRepository>
    {
        private readonly IAtletaRepository _atletaRepository;
        private readonly IAvaliacaoNutricionalService _avaliacaoService;
        public EditModel(IAvaliacaoNutricionalRepository repository,
                         IAtletaRepository atletaRepository,
                         IAvaliacaoNutricionalService avaliacaoService,
                         IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _atletaRepository = atletaRepository;
            _avaliacaoService = avaliacaoService;
        }

        [BindProperty]
        public AvaliacaoNutricional AvaliacaoNutricional { get; set; } = default!;

        [BindProperty]
        public ArquivoVM ArquivoImagem { get; set; } = new();

        [BindProperty]
        public ArquivoVM ArquivoImagemCostas { get; set; } = new();

        [BindProperty]
        public List<AvaliacaoNutricionalAnexoVM> Anexos { get; set; } = new();

        private void BindSelectLists()
        {

            Atletas = _atletaRepository.Query()
                            .Where(a => a.Ativo)
                            .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                            .OrderBy(a => a.Text)
                            .ToList();
        }
        public List<SelectListItem> Atletas { get; set; } = new();


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            BindSelectLists();

            if (id == null)
            {
                return NotFound();
            }

            //Método com o Include
            var avaliacaoNutricional = await _repository.GetForUpdateAsync(id ?? throw new ArgumentException(nameof(id)));

            if (avaliacaoNutricional == null)
            {
                return NotFound();
            }

            if(avaliacaoNutricional.Peso.HasValue)
                avaliacaoNutricional.PesoAsString = avaliacaoNutricional.Peso.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            if(avaliacaoNutricional.Altura.HasValue)
                avaliacaoNutricional.AlturaAsString = avaliacaoNutricional.Altura.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            if (avaliacaoNutricional.PctGordura.HasValue)
                avaliacaoNutricional.PctGorduraAsString = avaliacaoNutricional.PctGordura.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            if (avaliacaoNutricional.MassaMuscular.HasValue)
                avaliacaoNutricional.MassaMuscularAsString = avaliacaoNutricional.MassaMuscular.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            if (avaliacaoNutricional.MassaLivre.HasValue)
                avaliacaoNutricional.MassaLivreAsString = avaliacaoNutricional.MassaLivre.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            if (avaliacaoNutricional.GorduraViceral.HasValue)
                avaliacaoNutricional.GorduraViceralAsString = avaliacaoNutricional.GorduraViceral.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            if (avaliacaoNutricional.AguaCorporal.HasValue)
                avaliacaoNutricional.AguaCorporalAsString = avaliacaoNutricional.AguaCorporal.Value.ToString(new System.Globalization.CultureInfo("pt-BR"));

            Anexos = avaliacaoNutricional.Anexos
                                .Select(a => new AvaliacaoNutricionalAnexoVM
                                {
                                    Id = a.Id,
                                    Descricao = a.Descricao,
                                    MarcadoParaExclusao = false
                                })
                                .ToList();

            AvaliacaoNutricional = avaliacaoNutricional;

            return Page();
        }

        public async Task<IActionResult> OnGetConteudoDoArquivoAsync(int avaliacaoId)
        {
            var avaliacao = await _repository.Query()
                                .Include(x => x.ArquivoImagem)
                                .FirstOrDefaultAsync(x => x.Id == avaliacaoId);

            if (avaliacao?.ArquivoImagem == null)
                return NotFound();

            return File(
                avaliacao.ArquivoImagem.Conteudo,
                avaliacao.ArquivoImagem.ContentType
            );
        }

        public async Task<IActionResult> OnGetConteudoDoArquivoCostasAsync(int avaliacaoId)
        {
            var avaliacao = await _repository.Query()
                                .Include(x => x.ArquivoImagemCostas)
                                .FirstOrDefaultAsync(x => x.Id == avaliacaoId);

            if (avaliacao?.ArquivoImagemCostas == null)
                return NotFound();

            return File(
                avaliacao.ArquivoImagemCostas.Conteudo,
                avaliacao.ArquivoImagemCostas.ContentType
            );
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            //return RedirectToPage("./Index");
            IFL.WebApp.Areas.Admin.Pages.General.AvaliacoesNutricional.CreateModel.ValidarValor(AvaliacaoNutricional);
            AvaliacaoNutricional.Atleta = _atletaRepository.Query().Where(a => AvaliacaoNutricional.AtletaId == a.Id).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                BindSelectLists();
                return Page();
            }

            try
            {
                await _avaliacaoService.UpdateAsync(AvaliacaoNutricional, ArquivoImagem, ArquivoImagemCostas, Anexos);
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

        public async Task<IActionResult> OnGetDownloadDoAnexoAsync(int avaliacaoId, int anexoId)
        {
            var avaliacaoNutricional = await _repository.Query()
                                                .Include(x => x.Anexos)
                                                .ThenInclude(x => x.Anexo)
                                            .FirstOrDefaultAsync(x => x.Id == avaliacaoId && x.Anexos.Any(a => a.Id == anexoId));

            if (avaliacaoNutricional == null)
                return NotFound();

            var anexoDoLivro = avaliacaoNutricional.Anexos.FirstOrDefault(x => x.Id == anexoId);

            if (anexoDoLivro?.Anexo == null)
                return NotFound();

            return File(
                anexoDoLivro.Anexo.Conteudo,
                anexoDoLivro.Anexo.ContentType,
                anexoDoLivro.Anexo.NomeOriginal
            );
        }
    }
}
