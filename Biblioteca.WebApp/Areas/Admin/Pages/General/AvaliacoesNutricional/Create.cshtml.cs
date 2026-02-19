using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Exceptions;
using IFL.WebApp.Infrastructure.Extensions;
using IFL.WebApp.Infrastructure.Pages;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Infrastructure.Services;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Areas.Admin.Pages.General.AvaliacoesNutricional
{
    public class CreateModel : CrudPageModel<AvaliacaoNutricional, IAvaliacaoNutricionalRepository>
    {
        private readonly IAtletaRepository _atletaRepository;
        private readonly IAvaliacaoNutricionalService _avaliacaoService;
        public CreateModel(IAvaliacaoNutricionalRepository repository,
                           IAtletaRepository atletaRepository,
                           IAvaliacaoNutricionalService avaliacaoService,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            _atletaRepository = atletaRepository;
            _avaliacaoService = avaliacaoService;
        }

        public IActionResult OnGet()
        {
            BindSelectLists();
            return Page();
        }

        private void BindSelectLists()
        {
             Atletas = _atletaRepository.Query()
                            .Where(a => a.Ativo)
                            .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nome })
                            .OrderBy(a => a.Text)
                            .ToList();
        }

        public async Task<IActionResult> OnGetConteudoDoArquivoAsync(int avalicacaoId)
        {
            var avaliacao = await _repository.Query()
                                .Include(x => x.ArquivoImagem)
                                .FirstOrDefaultAsync(x => x.Id == avalicacaoId);

            if (avaliacao?.ArquivoImagem == null)
                return NotFound();

            return File(
                avaliacao.ArquivoImagem.Conteudo,
                avaliacao.ArquivoImagem.ContentType
            );
        }

        public List<SelectListItem> Atletas { get; set; } = new();

        [BindProperty]
        public AvaliacaoNutricional AvaliacaoNutricional { get; set; } = default!;

        [BindProperty]
        public ArquivoVM ArquivoImagem { get; set; } = new();

        [BindProperty]
        public ArquivoVM ArquivoImagemCostas { get; set; } = new();

        [BindProperty]
        public List<AvaliacaoNutricionalAnexoVM> Anexos { get; set; } = new();

        public static void ValidarValor(AvaliacaoNutricional avaliacaoNutricional)
        {
            if (!avaliacaoNutricional.PesoAsString.TryParseValor(out string errorMessage, out var valorDecimal))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.PesoAsString", errorMessage));
            }

            avaliacaoNutricional.Peso = valorDecimal;

            if (!avaliacaoNutricional.AlturaAsString.TryParseValor(out string errorMessage2, out var valorDecimal2))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.AlturaAsString", errorMessage2));
            }

            avaliacaoNutricional.Altura = valorDecimal2;

            if (!avaliacaoNutricional.PctGorduraAsString.TryParseValor(out string errorMessage3, out var valorDecimal3))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.PctGorduraAsString", errorMessage3));
            }

            avaliacaoNutricional.PctGordura = valorDecimal3;

            if (!avaliacaoNutricional.MassaMuscularAsString.TryParseValor(out string errorMessage4, out var valorDecimal4))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.MassaMuscularAsString", errorMessage4));
            }

            avaliacaoNutricional.MassaMuscular = valorDecimal4;

            if (!avaliacaoNutricional.MassaLivreAsString.TryParseValor(out string errorMessage5, out var valorDecimal5))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.MassaLivreAsString", errorMessage5));
            }

            avaliacaoNutricional.MassaLivre = valorDecimal5;

            if (!avaliacaoNutricional.GorduraViceralAsString.TryParseValor(out string errorMessage6, out var valorDecimal6))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.GorduraViceralAsString", errorMessage6));
            }

            avaliacaoNutricional.GorduraViceral = valorDecimal6;

            if (!avaliacaoNutricional.AguaCorporalAsString.TryParseValor(out string errorMessage7, out var valorDecimal7))
            {
                throw new Infrastructure.Exceptions.ValidationListException(("Pesagem.AguaCorporalAsString", errorMessage6));
            }

            avaliacaoNutricional.AguaCorporal = valorDecimal7;

        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()            
        {
            ValidarValor(AvaliacaoNutricional);
            AvaliacaoNutricional.Atleta = _atletaRepository.Query().Where(a => AvaliacaoNutricional.AtletaId == a.Id).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                BindSelectLists();
                return Page();
            }

            try
            {
                await _avaliacaoService.AddAsync(AvaliacaoNutricional, ArquivoImagem, ArquivoImagemCostas, Anexos);
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
