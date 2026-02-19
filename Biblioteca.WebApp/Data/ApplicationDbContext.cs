using IFL.WebApp.Model;
using IFL.WebApp.Model.Views;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IFL.WebApp.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) 
        {

        }


        public DbSet<Assunto> Assuntos { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<PrecoDeVenda> PrecosDeVenda { get; set; }

        //IFL
        public DbSet<Atleta> Atletas { get; set; }
        public DbSet<Modalidade> Modalidades { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Pesagem> Pesagens { get; set; }
        public DbSet<AvaliacaoNutricional> AvaliacoesNutricionais { get; set; }
        public DbSet<Arquivo> Arquivos { get; set; }

        public DbSet<EstatisticaCompeticao> EstatisticasCompeticao { get; set; }

        public DbSet<ReportLivrosView> ReportLivrosViewSet => Set<ReportLivrosView>();
                
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AvaliacaoNutricional>()
                    .HasOne(l => l.ArquivoImagem)
                    .WithMany()
                    .HasForeignKey(l => l.ArquivoImagemId)
                    .OnDelete(DeleteBehavior.NoAction); // Evita erro de múltiplos caminhos de cascata

            // Mapeamento para a Foto de Costas (ArquivoImagemCostas)
            builder.Entity<AvaliacaoNutricional>()
                .HasOne(l => l.ArquivoImagemCostas)
                .WithMany()
                .HasForeignKey(l => l.ArquivoImagemCostasId)
                .OnDelete(DeleteBehavior.NoAction); // Mantém consistência

            builder.Entity<Pesagem>()
                .Property(c => c.Peso1)
                .HasColumnType("decimal")
                .HasPrecision(6, 2);

            builder.Entity<Pesagem>()
                .Property(c => c.Peso2)
                .HasColumnType("decimal")
                .HasPrecision(6, 2);

            builder.Entity<Pesagem>()
                .Property(c => c.Peso3)
                .HasColumnType("decimal")
                .HasPrecision(6, 2);

            builder.Entity<Livro>()
                .Property(c => c.Valor)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            builder.Entity<Livro>()
                .HasMany(p => p.Autores)
                .WithMany(p => p.Livros);

            builder.Entity<Livro>()
                .HasMany(p => p.Assuntos)
                .WithMany(p => p.Livros);

            builder.Entity<ReportLivrosView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_ReportLivros");
            });

            builder.Entity<Atleta>()
                .HasMany(p => p.AtletaGrades);

            builder.Entity<AvaliacaoNutricional>()
                .HasMany(p => p.Anexos);

            builder.Entity<EstatisticaCompeticao>()
                .HasMany(p => p.Detalhes);

        }
    }
}
