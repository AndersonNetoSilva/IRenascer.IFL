using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class AssuntoRepository : DbContextRepository<Assunto, ApplicationDbContext>, IAssuntoRepository
    {
        public AssuntoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
