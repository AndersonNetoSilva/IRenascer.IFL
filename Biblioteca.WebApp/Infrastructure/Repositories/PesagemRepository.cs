using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class PesagemRepository : DbContextRepository<Pesagem, ApplicationDbContext>, IPesagemRepository
    {
        public PesagemRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<Pesagem?> GetForUpdateAsync(int? id)
        {
            return await Query()
                .Include(x => x.Atleta)
                .Include(x => x.Evento)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
