using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FCG.Infrastructure.Data;

public class DbFcg : DbContext
{
    public DbFcg(DbContextOptions<DbFcg> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PerfilEntity>().ToTable("USERS");
        modelBuilder.Entity<CarteiraEntity>().ToTable("CARTEIRAS");
        modelBuilder.Entity<JogosEntity>().ToTable("JOGOS");
        modelBuilder.Entity<BibliotecaEntity>().ToTable("BIBLIOTECA_DE_JOGOS");
        modelBuilder.Entity<PromocoesEntity>().ToTable("PROMOCOES");
        modelBuilder.Entity<ILoggerEnt>().ToTable("LOGS");

        modelBuilder.Entity<CarteiraEntity>()
            .Property(c => c.Saldo)
            .HasPrecision(18, 2);

        modelBuilder.Entity<JogosEntity>()
            .Property(j => j.Preco)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PromocoesEntity>()
            .Property(j => j.Valor)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PromocoesEntity>()
            .Property(p => p.IdJogos)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null));
    }

    public DbSet<PerfilEntity> USERS { get; set; }
    public DbSet<CarteiraEntity> CARTEIRAS { get; set; }
    public DbSet<JogosEntity> JOGOS { get; set; }
    public DbSet<BibliotecaEntity> BIBLIOTECA_DE_JOGOS { get; set; }
    public DbSet<PromocoesEntity> PROMOCOES { get; set; }
    public DbSet<ILoggerEnt> LOGS { get; set; }
}
