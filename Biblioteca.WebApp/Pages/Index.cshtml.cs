using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.IO;

namespace IFL.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILivroRepository _livroRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IAtletaRepository _atletaRepository;

        public IndexModel(ILogger<IndexModel> logger, 
                          ILivroRepository    livroRepository,
                          IEventoRepository   eventoRepository,
                          IAtletaRepository   atletaRepository)
        {
            _logger = logger;
            _livroRepository  = livroRepository;
            _eventoRepository = eventoRepository;
            _atletaRepository = atletaRepository;
        }

        public List<AutoresPorLivro> AutoresPorLivroList { get; set; }

        public List<EventoDTO> EventoDTOList { get; set; }
        public List<DonutsModel> DonutsModeList { get; set; }
        public List<DonutsModel> SexoDonutsModeList { get; set; }
        public Notificacoes Notificacoes { get; set; }
        public Notificacoes EventosMes { get; set; }
        public List<DadosChartDTO> ClassesDTOList { get; set; }
        public List<DadosChartDTO> EscolaDTOList { get; set; }
        public List<DadosChartDTO> ClassesFemininaDTOList { get; set; }
        public List<DadosChartDTO> ClassesMasculinaDTOList { get; set; }
        public void OnGet()
        {
            AutoresPorLivroList = GetAutoresPorLivroList();
            EventoDTOList       = GetEventoDtoList();
            EscolaDTOList       = GetEscolaChartDTO();

            DonutsModeList      = GetDonutsModelList();
            SexoDonutsModeList  = GetSexoDonutsModelList();
            Notificacoes        = GetNotificacoes();
            EventosMes          = GetEventoMes();
            ClassesDTOList      = GetClasseChartDTO();
            ClassesFemininaDTOList =  GetClasseChartDTO(Sexo.Feminino);
            ClassesMasculinaDTOList = GetClasseChartDTO(Sexo.Masculino);
        }

        private Notificacoes GetNotificacoes()
        {
            var inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var fim = inicio.AddMonths(1).AddDays(-1);

            return new Notificacoes()
            {
                //AniversariantesDoMes = "asdfas fdasfsdafsda"
                AniversariantesDoMes = string.Join(", ",
                                                        _atletaRepository
                                                            .Query()
                                                            .Where(x => x.DataNascimento.Month == DateTime.Now.Month)
                                                            .Select(x => $"{x.Nome} ({x.DataNascimento:dd/MM})")
                                                            .ToList()
                                                    )

            };
        }

        private Notificacoes GetEventoMes()
        {
            var inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var fim = inicio.AddMonths(1).AddDays(-1); 

            return new Notificacoes()
            {
                QtdEventosMes = _eventoRepository
                                .Query()
                                .Where(x=> x.Data >= inicio && x.Data <= fim)
                                .ToList()
                                .Count()
            };
        }

        private List<DonutsModel> GetSexoDonutsModelList()
        {
            return _atletaRepository
                .Query()
                .GroupBy(x => x.Sexo)
                .Select(x => new DonutsModel(x.Count(), x.Key.ToString()))
                .ToList();
        }

        private List<DonutsModel> GetDonutsModelList()
        {
            return _livroRepository
                .Query()
                .Include(x => x.Autores)
                .GroupBy(x => x.Titulo)
                .Select(x => new DonutsModel(x.Select(y => y.Autores.Count()).First(), x.Key))
                .ToList();
        }

        private List<AutoresPorLivro> GetAutoresPorLivroList()
        {
            return _livroRepository
                .Query()
                .Include(x => x.Autores)
                .GroupBy(x => x.Titulo)
                .Select(x => new AutoresPorLivro(x.Select(y => y.Autores.Count()).First(), x.Key))
                .ToList();
        }

        private List<EventoDTO> GetEventoDtoList()
        {
            return _eventoRepository
                        .Query()
                        .Where(x=> x.Data.Year == DateTime.Now.Year)
                        .OrderBy(x => x.Data)
                        .Select(x => new EventoDTO(x.Data, x.Nome))
                        .ToList();
        }

        private List<DadosChartDTO> GetClasseChartDTO(Sexo sexo = Sexo.NaoInformar)
        {
            var lista = _atletaRepository
                            .Query()
                            .ToList();

            if (sexo != Sexo.NaoInformar)
                lista = lista.Where(x => x.Sexo == sexo).ToList();

            lista = lista.OrderBy(x => x.Classe).ToList();

            var result = lista
                            .GroupBy(x => x.Classe)
                            .Select(x => new DadosChartDTO(x.Count(), x.Key.ToString()))
                            .ToList();

            return result;
        }

        private List<DadosChartDTO> GetEscolaChartDTO()
        {
            var lista = _atletaRepository
                            .Query()
                            .ToList();

            lista = lista.OrderBy(x => x.TipoEscola).ToList();

            var result = lista
                            .GroupBy(x => x.TipoEscola)
                            .Select(x => new DadosChartDTO(x.Count(), x.Key.ToString()))
                            .ToList();

            return result;
        }

        public void SalvarDatasetEmTxt(List<DadosChartDTO> dataset, string name)
        {
            var json = JsonSerializer.Serialize(dataset, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var caminho = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "arquivos",
                name
            );

            Directory.CreateDirectory(Path.GetDirectoryName(caminho)!);

            File(caminho, json);
        }

    }

    [Serializable]
    public class AutoresPorLivro
    {
        public AutoresPorLivro()
        {

        }
        public AutoresPorLivro(int quantidade, string livro)
        {
            Quantidade = quantidade;
            Livro = livro;
        }

        public int Quantidade { get; set; }

        public string Livro { get; set; }
    }

    [Serializable]
    public class DadosChartDTO
    {
        public DadosChartDTO()
        {
            Label = string.Empty;
        }
        public DadosChartDTO(int quantidade, string label)
        {
            Quantidade = quantidade;
            Label = label;
        }

        public int Quantidade { get; set; }
        public string Label { get; set; }
    }

    public class DadosTempoChartDTO
    {
        public DadosTempoChartDTO()
        {
            Label = string.Empty;
        }
        public DadosTempoChartDTO(TimeSpan tempo, string label)
        {
            Tempo = tempo;
            Label = label;
        }

        public TimeSpan Tempo { get; set; }
        public string Label { get; set; }
    }


    [Serializable]
    public class EventoDTO
    {
        public EventoDTO()
        {

        }
        public EventoDTO(DateTime data, string evento)
        {
            Data   = data.ToString("dd/MM/yyyy"); ;
            Evento = evento;
        }

        public string Data { get; set; }
        public string Evento { get; set; }
    }

    [Serializable]
    public class Notificacoes
    {
        public string AniversariantesDoMes { get; set; }
        public int QtdEventosMes { get; set; }
    }

    [Serializable]
    public class DonutsModel
    {
        public DonutsModel()
        {

        }
        public DonutsModel(int value, string label)
        {
            Value = value;
            Label = label;
        }

        public int Value { get; set; }

        public string Label { get; set; }
    }

    [Serializable]
    public class DadosPontuacaoChartDTO
    {
        public DadosPontuacaoChartDTO()
        {
            
        }
        public DadosPontuacaoChartDTO(string luta, int yuko, int shido, int wazari, int ippon)
        {
            Luta    = luta;
            Yuko    = yuko;
            Shido   = shido;  
            Wazari  = wazari;  
            Ippon   = ippon;
        }

        public string Luta { get; set; }
        public int Yuko { get; set; }
        public int Shido { get; set; }
        public int Wazari { get; set; }
        public int Ippon { get; set; }
    }

}
