using IFL.WebApp.Data;
using IFL.WebApp.Model;
using IFL.WebApp.Model.Views;
using IFL.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.Relatorios.Eventos
{
    public class IndexModel : PagerAndFilterPageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<ReportEventosView> ReportEventosViewList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (!Request.QueryString.HasValue)
            {
                ReportEventosViewList = new List<ReportEventosView>();
                return;
            }

            int pageSize = 1000;

            var query = _dbContext
                            .Eventos
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var campoValorList = Search.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(f => f.Split(':', 2))
                                    .Where(a => a.Length == 2)
                                    .Select(a => new { Campo = a[0].Trim(), Valor = a[1].Trim() });

                foreach (var f in campoValorList)
                {
                    switch (f.Campo.ToLower())
                    {
                        case "nome":
                            query = query.Where(w => w.Nome.Contains(f.Valor));
                            break;
                        case "data":
                            if (DateTime.TryParse(f.Valor, out DateTime dataFiltro))
                            {
                                // Filtra ignorando as horas (apenas a data)
                                query = query.Where(w => w.Data.Date == dataFiltro.Date);
                            }
                            break;
                        case "ano":
                            if (int.TryParse(f.Valor, out int anoFiltro))
                            {
                                // Filtra ignorando as horas (apenas a data)
                                query = query.Where(w => w.Data.Date.Year == anoFiltro);
                            }
                            break;
                    }
                }
            }

            int total = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            var viewResultSet = await query
                                        .Skip((PageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            ReportEventosViewList =
                viewResultSet
                .Select(x => new ReportEventosView()
                    {
                        Nome = x.Nome,
                        Local = x.Local,
                        Data = x.Data.ToString("dd/MM/yyyy"),
                        Descricao = x.Descricao,
                        TipoEvento = x.TipoEvento == null ? "Não informado" : x.TipoEvento.ToString(),
                        Encerrado = x.Encerrado
                    })
                .OrderBy(y => y.Data)
                .ToList();
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
