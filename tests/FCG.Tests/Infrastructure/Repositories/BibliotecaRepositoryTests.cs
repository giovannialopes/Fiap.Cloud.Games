using System;
using System.Threading.Tasks;
using FCG.Domain.Entities;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FCG.Tests.Infrastructure.Repositories;

public class BibliotecaRepositoryTests
{
    private DbFcg CreateContext() {
        var options = new DbContextOptionsBuilder<DbFcg>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new DbFcg(options);
    }

    [Fact]
    public async Task AdicionaNaBiblioteca_DevePersistirEntidade() {
        // Arrange
        using var context = CreateContext();
        var repo = new BibliotecaRepository(context);
        var entity = BibliotecaEntity.Criar(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await repo.AdicionaNaBiblioteca(entity);

        // Assert
        var encontrado = await context.BIBLIOTECA_DE_JOGOS.FirstOrDefaultAsync(x => x.Id == entity.Id);
        Assert.NotNull(encontrado);
        Assert.Equal(entity.JogoId, encontrado.JogoId);
        Assert.Equal(entity.UsuarioId, encontrado.UsuarioId);
    }

    [Fact]
    public async Task Commit_DeveSalvarAlteracoes() {
        // Arrange
        using var context = CreateContext();
        var repo = new BibliotecaRepository(context);
        var entity = BibliotecaEntity.Criar(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await context.BIBLIOTECA_DE_JOGOS.AddAsync(entity);
        await repo.Commit();

        // Assert
        var encontrado = await context.BIBLIOTECA_DE_JOGOS.FirstOrDefaultAsync(x => x.Id == entity.Id);
        Assert.NotNull(encontrado);
    }
}
