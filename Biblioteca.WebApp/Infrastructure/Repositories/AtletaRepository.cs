using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class AtletaRepository : DbContextRepository<Atleta, ApplicationDbContext>, IAtletaRepository
    {
        public AtletaRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<Atleta?> GetForUpdateAsync(int? id)

        {
            return await Query()
                .Include(x => x.AtletaGrades)
                .Include(x => x.ArquivoImagem)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public  bool ExiteAtletaNaModalidade(int? modalidadeId)

        {
            return  Query()
                      .Any(x => x.AtletaGrades.Any(g=> g.ModalidadeId == modalidadeId));
        }

    }
}
