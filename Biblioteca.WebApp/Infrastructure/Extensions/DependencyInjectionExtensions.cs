using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Infrastructure.Services;

namespace IFL.WebApp.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Serviços
            services.AddTransient<IReportService, ReportService>();

            // Repositórios
            services.AddScoped<IAssuntoRepository, AssuntoRepository>();
            services.AddScoped<IAutorRepository,   AutorRepository>();
            services.AddScoped<ILivroRepository,   LivroRepository>();
            services.AddScoped<IAtletaRepository,  AtletaRepository>();
            services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
            services.AddScoped<IModalidadeRepository,  ModalidadeRepository>();
            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<IHorarioRepository, HorarioRepository>();
            services.AddScoped<IPesagemRepository, PesagemRepository>();
            services.AddScoped<IAvaliacaoNutricionalRepository, AvaliacaoNutricionalRepository>();
            services.AddScoped<IAvaliacaoNutricionalService, AvaliacaoNutricionalService>();
            services.AddScoped<IPrecoDeVendaRepository, PrecoDeVendaRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

            return services;
        }
    }
}
