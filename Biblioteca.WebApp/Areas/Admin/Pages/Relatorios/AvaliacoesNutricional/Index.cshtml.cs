using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Model;
using IFL.WebApp.Model.Views;
using IFL.WebApp.PageModels;
using IFL.WebApp.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Drawing;

namespace IFL.WebApp.Areas.Admin.Pages.Relatorios.AvaliacoesNutricional
{
    public class IndexModel : PagerAndFilterPageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAtletaRepository _atletaRepository;
        private readonly IAvaliacaoNutricionalRepository _avaliacaoNutricionalRepository;
        private readonly IPesagemRepository _pesagemRepository;
        
        public IndexModel(ApplicationDbContext dbContext, 
                            IAtletaRepository atletaRepository,
                            IAvaliacaoNutricionalRepository avaliacaoNutricionalRepository,
                            IPesagemRepository pesagemRepository)
        {
            _dbContext = dbContext;
            _atletaRepository = atletaRepository;
            _avaliacaoNutricionalRepository = avaliacaoNutricionalRepository;
            _pesagemRepository= pesagemRepository;
        }

        public IList<ReportAvaliacoesNutricionaisView> ReportAvaliacoesNutricionaisList { get; set; } = default!;
        public ReportAtletasView _ReportAtletasVW { get; set; } = default!;

        public ReportAvaliacoesNutricionaisView _ReportAvaliacoesNutricionaisVW { get; set; } = default!;

        public List<DadosDecimalChartDTO> PesoDTOList { get; set; }

        
        [BindProperty]
        public ArquivoVM ArquivoImagem { get; set; } = new();
        public TimeSpan TempoMedioLutas { get; set; }
        public int QtdGoldemScores { get; set; }

        public async Task OnGetAsync()
        {

            if (!Request.QueryString.HasValue)
            {
                ReportAvaliacoesNutricionaisList = new List<ReportAvaliacoesNutricionaisView>();
                return;
            }

            int idAtleta = int.TryParse(Request.Query["idAtleta"], out var v) ? v : 0;
            int idAvaliacao = int.TryParse(Request.Query["idAvaliacao"], out  v) ? v : 0;

            var query = _dbContext
                            .AvaliacoesNutricionais
                            .AsQueryable()
                            .Include(x => x.Atleta)
                            .Include(x => x.Anexos)
                            .Where(x=> x.AtletaId == idAtleta && x.Id == idAvaliacao);

            _ReportAtletasVW =
                              query
                                    .Select(x => new ReportAtletasView()
                                        {
                                            Id              = x.Atleta.Id,
                                            Nome            = x.Atleta.Nome,
                                            ArquivoImagemId = x.Atleta.ArquivoImagemId,
                                            Endereco        = x.Atleta.Bairro + " " + x.Atleta.Municipio,
                                            DataNascimento  = x.Atleta.DataNascimento.ToString("dd/MM/yyyy"),
                                            Idade           = x.Atleta.Idade,
                                            TelefonePrincipal = FormatarTelefone(x.Atleta.TelefonePrincipal),
                                            Graduacao         = x.Atleta.GraduacaoAsString                                             
                                    })
                                    .OrderBy(y => y.Nome)
                                    .FirstOrDefault();

            _ReportAvaliacoesNutricionaisVW =
                              query
                                    .Select(x => new ReportAvaliacoesNutricionaisView()
                                    {
                                        Id = x.Id,
                                        Nome = x.Atleta.Nome,
                                        ArquivoImagemFrenteId = x.ArquivoImagemId,
                                        ArquivoImagemCostasId = x.ArquivoImagemCostasId,
                                        AguaCorporal = x.AguaCorporal,
                                        Altura  =  x.Altura,
                                        Data = x.Data.Value.ToString("dd/MM/yyyy"),
                                        GorduraViceral = x.GorduraViceral,
                                        MassaLivre = x.MassaLivre,
                                        MassaMuscular = x.MassaMuscular,
                                        Obs = x.Obs,
                                        PctGordura = x.PctGordura,
                                        Peso =x.Peso
                                    })
                                    .OrderBy(y => y.Nome)
                                    .FirstOrDefault();

            PesoDTOList = GetPesoAtletaDTO(idAtleta);

            
        }

        private List<DadosDecimalChartDTO> GetPesoAtletaDTO(int idAtleta)
        {
            List<DadosDecimalChartDTO> returnList = new List<DadosDecimalChartDTO>();

            Pesagem pesagem = _pesagemRepository
                                .Query()
                                .Where(x => x.AtletaId == idAtleta )
                                .OrderBy(x=> x.Evento.Data)
                                .FirstOrDefault();

            if (pesagem != null)
            {
                returnList.Add(new DadosDecimalChartDTO(pesagem.Peso1.Value, pesagem.Pesagem1.Value.ToString("dd/MM/yyyy")));
                returnList.Add(new DadosDecimalChartDTO(pesagem.Peso2.Value, pesagem.Pesagem2.Value.ToString("dd/MM/yyyy")));
                returnList.Add(new DadosDecimalChartDTO(pesagem.Peso3.Value, pesagem.Pesagem3.Value.ToString("dd/MM/yyyy")));
            }

            return returnList;

        }

        private List<DonutsModel> GetSexoDonutsModelList()
        {
            return _atletaRepository
                        .Query()
                        .GroupBy(x => x.Sexo)
                        .Select(x => new DonutsModel(x.Count(), x.Key.ToString()))
                        .ToList();
        }

        public async Task<IActionResult> OnGetConteudoDoArquivoAsync(int atletaId)
        {
            var avaliacao = await _atletaRepository
                                    .Query()
                                    .Include(x => x.ArquivoImagem)
                                    .FirstOrDefaultAsync(x => x.Id == atletaId);

            if (avaliacao?.ArquivoImagem == null)
                return NotFound();

            return File(
                avaliacao.ArquivoImagem.Conteudo,
                avaliacao.ArquivoImagem.ContentType
            );
        }

        public async Task<IActionResult> OnGetConteudoFotoNutriAsync(int avaliacaoId, int idArquivo)
        {
            var avaliacao = await _avaliacaoNutricionalRepository
                                    .Query()
                                    .Include(x => x.ArquivoImagem)
                                    .FirstOrDefaultAsync(x => x.Id == avaliacaoId && x.ArquivoImagemId == idArquivo);

            if (avaliacao?.ArquivoImagem == null)
                return NotFound();

            return File(
                avaliacao.ArquivoImagem.Conteudo,
                avaliacao.ArquivoImagem.ContentType
            );
        }

        public async Task<IActionResult> OnGetConteudoFotoNutriCostasAsync(int avaliacaoId, int idArquivo)
        {
            var avaliacao = await _avaliacaoNutricionalRepository
                                    .Query()
                                    .Include(x => x.ArquivoImagemCostas)
                                    .FirstOrDefaultAsync(x => x.Id == avaliacaoId && x.ArquivoImagemCostasId == idArquivo);

            if (avaliacao?.ArquivoImagemCostas == null)
                return NotFound();

            return File(
                avaliacao.ArquivoImagemCostas.Conteudo,
                avaliacao.ArquivoImagemCostas.ContentType
            );
        }

        

        public static string FormatarTelefone(string? telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            // remove tudo que não for número
            var numeros = new string(telefone.Where(char.IsDigit).ToArray());

            if (numeros.Length == 11)
                return Convert.ToUInt64(numeros).ToString(@"(00) 00000-0000");

            if (numeros.Length == 10)
                return Convert.ToUInt64(numeros).ToString(@"(00) 0000-0000");

            // se não bater com padrão conhecido, retorna como veio
            return telefone;
        }
    }
}
