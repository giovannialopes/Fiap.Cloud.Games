using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCG.Domain.Entities;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FCG.Tests.Infrastructure.Repositories;

public class PromocoesRepositoryTests
{
    private DbFcg CreateContext() {
        var options = new DbContextOptionsBuilder<DbFcg>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new DbFcg(options);
    }

    [Fact]
    public async Task AdicionarPromocoes_DevePersistirEntidade() {
        // Arrange
        using var context = CreateContext();
        var repo = new PromocoesRepository(context);
        var idJogos = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var promocao = PromocoesEntity.Criar(
            "Promoção Teste",
            25.0m,
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(1),
            idJogos
        );

        // Act
        await repo.AdicionarPromocoes(promocao);

        // Assert
        var encontrada = await context.PROMOCOES.FirstOrDefaultAsync(x => x.Id == promocao.Id);
        Assert.NotNull(encontrada);
        Assert.Equal("Promoção Teste", encontrada.Nome);
        Assert.Equal(idJogos, encontrada.IdJogos);
    }

    [Fact]
    public async Task ConsultaPromocoesAtivas_DeveRetornarPromocaoAtiva() {
        // Arrange
        using var context = CreateContext();
        var repo = new PromocoesRepository(context);

        var idJogos = new List<Guid> { Guid.NewGuid() };
        var promocaoAtiva = PromocoesEntity.Criar(
            "Promoção Ativa",
            10.0m,
            DateTime.UtcNow.AddDays(-2),
            DateTime.UtcNow.AddDays(2),
            idJogos
        );
        var promocaoInativa = PromocoesEntity.Criar(
            "Promoção Inativa",
            5.0m,
            DateTime.UtcNow.AddDays(-10),
            DateTime.UtcNow.AddDays(-5),
            idJogos
        );

        await context.PROMOCOES.AddAsync(promocaoAtiva);
        await context.PROMOCOES.AddAsync(promocaoInativa);
        await context.SaveChangesAsync();

        // Act
        var resultado = await repo.ConsultaPromocoesAtivas();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Promoção Ativa", resultado.Nome);
        Assert.Equal(10.0m, resultado.Valor);
        Assert.Equal(idJogos, resultado.IdJogos);
    }

    [Fact]
    public async Task ConsultaPromocoesAtivas_DeveRetornarNull_QuandoNaoHaPromocaoAtiva() {
        // Arrange
        using var context = CreateContext();
        var repo = new PromocoesRepository(context);

        var idJogos = new List<Guid> { Guid.NewGuid() };
        var promocaoInativa = PromocoesEntity.Criar(
            "Promoção Inativa",
            5.0m,
            DateTime.UtcNow.AddDays(-10),
            DateTime.UtcNow.AddDays(-5),
            idJogos
        );

        await context.PROMOCOES.AddAsync(promocaoInativa);
        await context.SaveChangesAsync();

        // Act
        var resultado = await repo.ConsultaPromocoesAtivas();

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task Commit_DeveSalvarAlteracoes() {
        // Arrange
        using var context = CreateContext();
        var repo = new PromocoesRepository(context);
        var promocao = PromocoesEntity.Criar(
            "Promoção Commit",
            15.0m,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            new List<Guid> { Guid.NewGuid() }
        );

        // Act
        await context.PROMOCOES.AddAsync(promocao);
        await repo.Commit();

        // Assert
        var encontrada = await context.PROMOCOES.FirstOrDefaultAsync(x => x.Id == promocao.Id);
        Assert.NotNull(encontrada);
    }
}
