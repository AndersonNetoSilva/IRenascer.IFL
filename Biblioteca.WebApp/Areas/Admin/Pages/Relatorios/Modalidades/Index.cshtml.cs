using IFL.WebApp.Data;
using IFL.WebApp.Model;
using IFL.WebApp.Model.Views;
using IFL.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Areas.Admin.Pages.Relatorios.Modalidades
{
    public class IndexModel : PagerAndFilterPageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<ReportBaseView> ReportBaseViewList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (!Request.QueryString.HasValue)
            {
                ReportBaseViewList = new List<ReportBaseView>();
                return;
            }

            int pageSize = 1000;

            var query = _dbContext
                            .Modalidades
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

            ReportBaseViewList =
                viewResultSet
                .Select(x => new ReportBaseView()
                    {
                        Nome = x.Nome,
                        Descricao = x.Descricao,
                        Ativo     = x.Ativo
                })
                .OrderBy(y => y.Descricao)
                .ToList();
        }

    }
}
