using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class GraduacaoRepository : DbContextRepository<Graduacao, ApplicationDbContext>, IGraduacaoRepository
    {
        public GraduacaoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<Graduacao?> GetForUpdateAsync(int? id)
        {
            return await Query()
                .Include(x => x.GraduacaoAtletas)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
