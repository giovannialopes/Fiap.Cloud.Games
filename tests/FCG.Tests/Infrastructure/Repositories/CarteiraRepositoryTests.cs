using System;
using System.Threading.Tasks;
using FCG.Domain.Entities;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FCG.Tests.Infrastructure.Repositories;

public class CarteiraRepositoryTests
{
    private DbFcg CreateContext() {
        var options = new DbContextOptionsBuilder<DbFcg>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new DbFcg(options);
    }

    [Fact]
    public async Task AdicionaSaldo_DevePersistirEntidade() {
        // Arrange
        using var context = CreateContext();
        var repo = new CarteiraRepository(context);
        var carteira = CarteiraEntity.Criar(Guid.NewGuid(), 150.50m);

        // Act
        await repo.AdicionaSaldo(carteira);
        await repo.Commit();

        // Assert
        var encontrada = await context.CARTEIRAS.FirstOrDefaultAsync(x => x.Id == carteira.Id);
        Assert.NotNull(encontrada);
        Assert.Equal(carteira.UsuarioId, encontrada.UsuarioId);
        Assert.Equal(150.50m, encontrada.Saldo);
    }

    [Fact]
    public async Task AlteraSaldo_DeveAtualizarSaldo() {
        // Arrange
        using var context = CreateContext();
        var repo = new CarteiraRepository(context);
        var carteira = CarteiraEntity.Criar(Guid.NewGuid(), 100m);
        await context.CARTEIRAS.AddAsync(carteira);
        await context.SaveChangesAsync();

        // Act
        carteira.Saldo = 200m;
        repo.AlteraSaldo(carteira);

        // Assert
        var encontrada = await context.CARTEIRAS.FirstOrDefaultAsync(x => x.Id == carteira.Id);
        Assert.NotNull(encontrada);
        Assert.Equal(200m, encontrada.Saldo);
    }

    [Fact]
    public async Task ObtemSaldoPorId_DeveRetornarCarteiraCorreta() {
        // Arrange
        using var context = CreateContext();
        var repo = new CarteiraRepository(context);
        var usuarioId = Guid.NewGuid();
        var carteira = CarteiraEntity.Criar(usuarioId, 300m);
        await context.CARTEIRAS.AddAsync(carteira);
        await context.SaveChangesAsync();

        // Act
        var encontrada = await repo.ObtemSaldoPorId(usuarioId);

        // Assert
        Assert.NotNull(encontrada);
        Assert.Equal(usuarioId, encontrada.UsuarioId);
        Assert.Equal(300m, encontrada.Saldo);
    }

    [Fact]
    public async Task Commit_DeveSalvarAlteracoes() {
        // Arrange
        using var context = CreateContext();
        var repo = new CarteiraRepository(context);
        var carteira = CarteiraEntity.Criar(Guid.NewGuid(), 50m);

        // Act
        await context.CARTEIRAS.AddAsync(carteira);
        await repo.Commit();

        // Assert
        var encontrada = await context.CARTEIRAS.FirstOrDefaultAsync(x => x.Id == carteira.Id);
        Assert.NotNull(encontrada);
    }
}
