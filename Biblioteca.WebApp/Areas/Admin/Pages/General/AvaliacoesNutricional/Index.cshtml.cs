using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Model;
using IFL.WebApp.PageModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace IFL.WebApp.Areas.Admin.Pages.General.AvaliacoesNutricional
{
    public class IndexModel : PagerAndFilterPageModel
    {
        protected readonly IAvaliacaoNutricionalRepository _repository;
        protected readonly IAtletaRepository _atletaRepository;
        private readonly IAvaliacaoNutricionalService _avaliacaoService;
        public IndexModel(IAvaliacaoNutricionalRepository repository,
                          IAtletaRepository atletaRepository,
                          IAvaliacaoNutricionalService avaliacaoService)
        {
            _repository = repository;
            _atletaRepository = atletaRepository;
            _avaliacaoService = avaliacaoService;
        }

        public IList<AvaliacaoNutricional> AvaliacoesNutricional { get; set; } = default!;

        public async Task OnGetAsync()
        {
            int pageSize = 10;

            ///fazer o Include nas Entidades
            var query = _repository.Query()
                                .Include(x=>x.Atleta)
                                .OrderBy(w => w.Atleta.Nome)
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
                        }
                    }
                }
            }

            int total = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            AvaliacoesNutricional = await query                                
                                    .Skip((PageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();


        }
    }
}
