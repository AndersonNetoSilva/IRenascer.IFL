using IFL.WebApp.Data;
using IFL.WebApp.Model;
using IFL.WebApp.Model.Views;
using IFL.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.Relatorios.Colaboradores
{
    public class IndexModel : PagerAndFilterPageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<ReportColaboradoresView> ReportColaboradoresViewList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (!Request.QueryString.HasValue)
            {
                ReportColaboradoresViewList = new List<ReportColaboradoresView>();
                return;
            }

            int pageSize = 1000;

            var query = _dbContext
                            .Colaboradores
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var filtros = Search
                                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                                .Select(f => f.Trim().ToLower())
                                .ToList();

                if (filtros.Count == 1)
                {
                    query = query.Where(w =>
                                        w.Nome.ToLower().Contains(filtros[0]));
                }
                else
                {
                    query = query.Where(w =>
                                         filtros.Any(f => w.Nome.ToLower().Contains(f)));
                }
            }

            int total = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            var viewResultSet = await query
                                        .Skip((PageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            ReportColaboradoresViewList =
                viewResultSet
                .Select(x => new ReportColaboradoresView()
                    {
                        Nome = x.Nome,
                        Endereco = x.Logradouro + " " + x.Bairro + " " + x.Municipio,
                        DataNascimento = x.DataNascimento.ToString("dd/MM/yyyy"),
                        Tipo = x.TipoColaborador == null ? "Não informado" : x.TipoColaborador.ToString(),
                        TelefonePrincipal = FormatarTelefone(x.TelefonePrincipal),
                        Profissao = x.Profissao
                    })
                .OrderBy(y => y.Nome)
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
