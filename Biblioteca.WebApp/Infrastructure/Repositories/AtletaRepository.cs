using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class AtletaRepository : DbContextRepository<Atleta, ApplicationDbContext>, IAtletaRepository
    {
        public AtletaRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
