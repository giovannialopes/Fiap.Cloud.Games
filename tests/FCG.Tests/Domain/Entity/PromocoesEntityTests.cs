using FCG.Domain.Entities;

namespace FCG.Tests.Domain.Entity;

public class PromocoesEntityTests
{
    [Fact]
    public void Criar_DeveRetornarEntidadeComValoresCorretos() {
        // Arrange
        var nome = "Promoção de Inverno";
        decimal valor = 50.0m;
        var dataInicio = DateTime.UtcNow;
        var dataFim = dataInicio.AddDays(10);
        var idJogos = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        // Act
        var entity = PromocoesEntity.Criar(nome, valor, dataInicio, dataFim, idJogos);

        // Assert
        Assert.NotNull(entity);
        Assert.Equal(nome, entity.Nome);
        Assert.Equal(valor, entity.Valor);
        Assert.Equal(dataInicio, entity.DataInicio);
        Assert.Equal(dataFim, entity.DataFim);
        Assert.Equal(idJogos, entity.IdJogos);
        Assert.NotEqual(Guid.Empty, entity.Id);
    }

    [Fact]
    public void Criar_DeveGerarNovoIdParaCadaInstancia() {
        // Arrange
        var nome = "Promoção";
        decimal valor = 10.0m;
        var dataInicio = DateTime.UtcNow;
        var dataFim = dataInicio.AddDays(1);
        var idJogos = new List<Guid> { Guid.NewGuid() };

        // Act
        var entity1 = PromocoesEntity.Criar(nome, valor, dataInicio, dataFim, idJogos);
        var entity2 = PromocoesEntity.Criar(nome, valor, dataInicio, dataFim, idJogos);

        // Assert
        Assert.NotEqual(entity1.Id, entity2.Id);
    }
}
