using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class AvaliacaoNutricionalRepository : DbContextRepository<AvaliacaoNutricional, ApplicationDbContext>, IAvaliacaoNutricionalRepository
    {
        public AvaliacaoNutricionalRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<AvaliacaoNutricional?> GetForUpdateAsync(int? id)
        {
            return await Query()
                .Include(x => x.Atleta)
                .Include(x => x.ArquivoImagem)
                .Include(x=> x.Anexos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
