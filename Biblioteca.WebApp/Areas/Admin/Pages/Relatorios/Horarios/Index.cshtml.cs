using IFL.WebApp.Data;
using IFL.WebApp.Model;
using IFL.WebApp.Model.Views;
using IFL.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.Relatorios.Horarios
{
    public class IndexModel : PagerAndFilterPageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<ReportHorariosView> ReportHorariosViewList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (!Request.QueryString.HasValue)
            {
                ReportHorariosViewList = new List<ReportHorariosView>();
                return;
            }

            int pageSize = 1000;

            var query = _dbContext
                            .Horarios
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
                    }
                }
            }

            int total = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            var viewResultSet = await query
                                        .Skip((PageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            ReportHorariosViewList =
                viewResultSet
                .Select(x => new ReportHorariosView()
                    {
                        Nome = x.Nome,
                        Descricao = x.Descricao,
                        Segunda   = x.Segunda,
                        Terca     = x.Terca,
                        Quarta    = x.Quarta,
                        Quinta    = x.Quinta,
                        Sexta     = x.Sexta,
                        Sabado    = x.Sabado,
                        Ativo     = x.Ativo
                })
                .OrderBy(y => y.Descricao)
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
