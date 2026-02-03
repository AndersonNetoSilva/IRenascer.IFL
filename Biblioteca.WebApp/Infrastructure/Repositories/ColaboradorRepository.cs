using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class ColaboradorRepository : DbContextRepository<Colaborador, ApplicationDbContext>, IColaboradorRepository
    {
        public ColaboradorRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
