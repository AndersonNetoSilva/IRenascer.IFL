using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class AutorRepository : DbContextRepository<Autor, ApplicationDbContext>, IAutorRepository
    {
        public AutorRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
