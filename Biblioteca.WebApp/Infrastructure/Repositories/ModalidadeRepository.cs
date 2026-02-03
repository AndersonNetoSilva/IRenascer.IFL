using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class ModalidadeRepository : DbContextRepository<Modalidade, ApplicationDbContext>, IModalidadeRepository
    {
        public ModalidadeRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
