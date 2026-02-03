using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class EventoRepository : DbContextRepository<Evento, ApplicationDbContext>, IEventoRepository
    {
        public EventoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
