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

namespace IFL.WebApp.Areas.Admin.Pages.Relatorios.EstatisticasCompeticao
{
    public class IndexModel : PagerAndFilterPageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAtletaRepository _atletaRepository;
        private readonly IEstatisticaCompeticaoRepository _estatisticaRepository;

        public IndexModel(ApplicationDbContext dbContext, 
                            IAtletaRepository atletaRepository,
                            IEstatisticaCompeticaoRepository estatisticaRepository)
        {
            _dbContext = dbContext;
            _atletaRepository = atletaRepository;
            _estatisticaRepository = estatisticaRepository;
        }

        public IList<ReportAtletasView> ReportAtletasViewList { get; set; } = default!;
        public ReportAtletasView _ReportAtletasVW { get; set; } = default!;
        public List<DonutsModel> SexoDonutsModeList { get; set; }
        public List<DonutsModel> LutasDonutsModeList { get; set; }
        public List<DadosChartDTO> PontucaoDTOList { get; set; }

        [BindProperty]
        public ArquivoVM ArquivoImagem { get; set; } = new();

        public TimeSpan TempoMedioLutas { get; set; }

        public int QtdGoldemScores { get; set; }

        public async Task OnGetAsync()
        {

            if (!Request.QueryString.HasValue)
            {
                ReportAtletasViewList = new List<ReportAtletasView>();
                return;
            }

            int idAtleta = int.TryParse(Request.Query["idAtleta"], out var v) ? v : 0;
            int idEvento = int.TryParse(Request.Query["idEvento"], out v) ? v : 0;

            var query = _dbContext
                            .EstatisticasCompeticao
                            .AsQueryable()
                            .Include(x => x.Atleta)
                            .Include(x => x.Evento)
                            .Include(x => x.Detalhes)
                            .Where(x=> x.AtletaId == idAtleta && 
                                       x.EventoId == idEvento);

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

            SexoDonutsModeList  = GetSexoDonutsModelList();
            LutasDonutsModeList = GetLutasDonutsModelList(_ReportAtletasVW.Id);
            PontucaoDTOList     = GetPontuacaoChartDTO(_ReportAtletasVW.Id);
            GetInfo(_ReportAtletasVW.Id);
        }

        private void GetInfo(int? idAtleta)
        {
            EstatisticaCompeticao estatistica = _estatisticaRepository
                                                    .Query()
                                                    .Where(x => x.AtletaId == idAtleta)
                                                    .Include(x => x.Detalhes)
                                                    .FirstOrDefault();

            if (estatistica == null || !estatistica.Detalhes.Any())
                TempoMedioLutas = TimeSpan.Zero;

            var mediaTicks = estatistica.Detalhes
                                        .Where(x => x.TempoDaLuta != null)
                                        .Average(x => x.TempoDaLuta.Ticks);

            TempoMedioLutas = TimeSpan.FromTicks((long)mediaTicks);
            QtdGoldemScores = estatistica.Detalhes.Where(x => x.GoldenScore == true).Count();
                
        }

        private List<DadosChartDTO> GetPontuacaoChartDTO(int? idAtleta)
        {
            List<DadosChartDTO> returnList = new List<DadosChartDTO>();

            EstatisticaCompeticao estatistica = _estatisticaRepository
                                                        .Query()
                                                        .Where(x => x.AtletaId == idAtleta)
                                                        .Include(x => x.Detalhes)
                                                        .FirstOrDefault();

            if (estatistica != null)
            {
                int qtdIpon    = estatistica.Detalhes.Where(x => x.Vitoria == true && x.Ippon > 0).Count();
                int qtdWhazari = estatistica.Detalhes.Where(x => x.Vitoria == true && x.Ippon == 0 && x.Wazari > 0).Count();
                int qtdYuko    = estatistica.Detalhes.Where(x => x.Vitoria == true && x.Ippon == 0 && x.Wazari == 0).Count();

                returnList.Add(new DadosChartDTO(qtdIpon, "Ippon"));
                returnList.Add(new DadosChartDTO(qtdWhazari, "Wazari"));
                returnList.Add(new DadosChartDTO(qtdYuko, "Yuko"));
            }

            return returnList;

        }

        private List<DonutsModel> GetLutasDonutsModelList(int? idAtleta)
        {
            List<DonutsModel> returnList = new List<DonutsModel>();

            EstatisticaCompeticao estatistica = _estatisticaRepository
                                            .Query()
                                            .Where(x => x.AtletaId == idAtleta)
                                            .Include(x => x.Detalhes)
                                            .FirstOrDefault();
                                            
            if( estatistica != null)
            {
                int vitorias = estatistica.Detalhes.Where(x => x.Vitoria == true).Count();
                int derrotas = estatistica.Detalhes.Where(x => x.Vitoria == false).Count();

                returnList.Add(new DonutsModel(vitorias, "Vitórias"));
                returnList.Add(new DonutsModel(derrotas, "Derrotas"));
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
