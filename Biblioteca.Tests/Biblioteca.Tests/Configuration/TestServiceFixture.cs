using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Infrastructure.Abstractions.Services;
using IFL.WebApp.Infrastructure.Repositories;
using IFL.WebApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Tests.Configuration
{
    public class TestServiceFixture
    {
        public ServiceProvider ServiceProvider { get; }

        public TestServiceFixture()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Serviços
            services.AddTransient<IReportService, ReportService>();

            // Repositórios
            services.AddScoped<IAssuntoRepository, AssuntoRepository>();
            services.AddScoped<IAutorRepository, AutorRepository>();
            services.AddScoped<ILivroRepository, LivroRepository>();
            services.AddScoped<IPrecoDeVendaRepository, PrecoDeVendaRepository>();
            services.AddScoped<IAtletaRepository, AtletaRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
