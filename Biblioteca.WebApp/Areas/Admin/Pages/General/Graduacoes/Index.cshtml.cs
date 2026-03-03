using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;
using IFL.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace IFL.WebApp.Areas.Admin.Pages.General.Graduacoes
{
    public class IndexModel : PagerAndFilterPageModel
    {
        protected readonly IGraduacaoRepository _repository;
        public IndexModel(IGraduacaoRepository repository)
        {
            _repository = repository;
        }

        public IList<Graduacao> Graduacoes { get; set; } = default!;

        public async Task OnGetAsync()
        {
            int pageSize = 10;

            var query = _repository.Query()
                .OrderBy(w => w.Data)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var filtros = Search.Split(';');

                if (filtros.Count() == 1 && filtros[0].Split(':').Count() == 1)
                {
                    query = query.Where(w =>
                        w.Descricao.Contains(Search));
                }
                else
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
                                query = query.Where(w => w.Descricao.Contains(f.Valor));
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
            }

            int total = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            Graduacoes = await query
                                .Skip((PageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        }
    }
}
