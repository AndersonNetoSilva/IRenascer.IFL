using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class HorarioRepository : DbContextRepository<Horario, ApplicationDbContext>, IHorarioRepository
    {
        public HorarioRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
