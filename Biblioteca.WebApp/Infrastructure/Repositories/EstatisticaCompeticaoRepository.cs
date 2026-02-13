using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class EstatisticaCompeticaoRepository : DbContextRepository<EstatisticaCompeticao, ApplicationDbContext>, IEstatisticaCompeticaoRepository
    {
        public EstatisticaCompeticaoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<EstatisticaCompeticao?> GetForUpdateAsync(int? id)
        {
            return await Query()
                .Include(x => x.Atleta)
                .Include(x => x.Evento)
                .Include(x=> x.Detalhes)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
