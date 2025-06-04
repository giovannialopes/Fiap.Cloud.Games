using System;
using System.Threading.Tasks;
using FCG.Domain.Entities;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FCG.Tests.Infrastructure.Repositories;

public class JogosRepositoryTests
{
    private DbFcg CreateContext() {
        var options = new DbContextOptionsBuilder<DbFcg>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new DbFcg(options);
    }

    [Fact]
    public async Task AdicionarJogos_DevePersistirEntidade() {
        // Arrange
        using var context = CreateContext();
        var repo = new JogosRepository(context);
        var jogo = JogosEntity.Criar("Jogo Teste", "Descrição", 99.99m, "Ação", 10, true);

        // Act
        await repo.AdicionarJogos(jogo);

        // Assert
        var encontrado = await context.JOGOS.FirstOrDefaultAsync(x => x.Id == jogo.Id);
        Assert.NotNull(encontrado);
        Assert.Equal("Jogo Teste", encontrado.Nome);
        Assert.True(encontrado.Ativo);
    }

    [Fact]
    public async Task AtualizarJogos_DeveAtualizarEntidade() {
        // Arrange
        using var context = CreateContext();
        var repo = new JogosRepository(context);
        var jogo = JogosEntity.Criar("Jogo Atualizar", "Desc", 50m, "RPG", 5, true);
        await context.JOGOS.AddAsync(jogo);
        await context.SaveChangesAsync();

        // Act
        jogo.Descricao = "Nova Descrição";
        jogo.Preco = 75m;
        await repo.AtualizarJogos(jogo);

        // Assert
        var encontrado = await context.JOGOS.FirstOrDefaultAsync(x => x.Id == jogo.Id);
        Assert.NotNull(encontrado);
        Assert.Equal("Nova Descrição", encontrado.Descricao);
        Assert.Equal(75m, encontrado.Preco);
    }

    [Fact]
    public async Task ObterJogoPorNome_DeveRetornarJogoAtivo() {
        // Arrange
        using var context = CreateContext();
        var repo = new JogosRepository(context);
        var jogo = JogosEntity.Criar("Jogo Procurado", "Desc", 10m, "Puzzle", 2, true);
        await context.JOGOS.AddAsync(jogo);
        await context.SaveChangesAsync();

        // Act
        var encontrado = await repo.ObterJogoPorNome("Jogo Procurado");

        // Assert
        Assert.NotNull(encontrado);
        Assert.Equal(jogo.Id, encontrado.Id);
        Assert.True(encontrado.Ativo);
    }

    [Fact]
    public async Task ObterJogoPorNome_NaoRetornaJogoInativo() {
        // Arrange
        using var context = CreateContext();
        var repo = new JogosRepository(context);
        var jogo = JogosEntity.Criar("Jogo Inativo", "Desc", 10m, "Puzzle", 2, false);
        await context.JOGOS.AddAsync(jogo);
        await context.SaveChangesAsync();

        // Act
        var encontrado = await repo.ObterJogoPorNome("Jogo Inativo");

        // Assert
        Assert.Null(encontrado);
    }

    [Fact]
    public async Task Commit_DeveSalvarAlteracoes() {
        // Arrange
        using var context = CreateContext();
        var repo = new JogosRepository(context);
        var jogo = JogosEntity.Criar("Jogo Commit", "Desc", 20m, "Ação", 1, true);

        // Act
        await context.JOGOS.AddAsync(jogo);
        await repo.Commit();

        // Assert
        var encontrado = await context.JOGOS.FirstOrDefaultAsync(x => x.Id == jogo.Id);
        Assert.NotNull(encontrado);
    }
}
