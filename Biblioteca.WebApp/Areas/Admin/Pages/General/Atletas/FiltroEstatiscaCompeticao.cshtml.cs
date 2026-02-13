using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;
using IFL.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Xaml.Permissions;

namespace IFL.WebApp.Areas.Admin.Pages.General.Atletas
{
    public class FiltroEstatiscaCompeticaoModel : PagerAndFilterPageModel
    {
        
        protected readonly IEstatisticaCompeticaoRepository _repository;
        public FiltroEstatiscaCompeticaoModel(IEstatisticaCompeticaoRepository repository)
        {
            _repository = repository;
        }

        public IList<EstatisticaCompeticao> EstatisticasCompeticao { get; set; } = default!;

        public async Task OnGetAsync()
        {
            int pageSize = 10;

            var query = _repository.Query()
                            .Include(x=>x.Atleta)
                            .Include(x => x.Evento)
                            .Include(x => x.Detalhes)
                            .OrderBy(w => w.Atleta)
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var filtros = Search.Split(';');

                if (filtros.Count() == 1 && filtros[0].Split(':').Count() == 1)
                {
                    query = query.Where(w =>
                        w.Atleta.Nome.Contains(Search));
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
                                query = query.Where(w => w.Atleta.Nome.Contains(f.Valor));
                                break;
                            case "evento":
                                query = query.Where(w => w.Evento.Nome.Contains(f.Valor));
                                break;
                        }
                    }
                }
            }

            int total = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            EstatisticasCompeticao = await query
                                    .Skip((PageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        }
    }
}
