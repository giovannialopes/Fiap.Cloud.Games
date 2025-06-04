using FCG.Domain.Entities;

namespace FCG.Tests.Domain.Entity;

public class JogosEntityTests
{
    [Fact]
    public void Criar_DeveRetornarEntidadeComValoresCorretos() {
        // Arrange
        var nome = "Jogo Teste";
        var descricao = "Descrição do jogo";
        decimal preco = 99.99m;
        var tipo = "Aventura";
        long quantidade = 10;
        bool ativo = true;

        // Act
        var entity = JogosEntity.Criar(nome, descricao, preco, tipo, quantidade, ativo);

        // Assert
        Assert.NotNull(entity);
        Assert.Equal(nome, entity.Nome);
        Assert.Equal(descricao, entity.Descricao);
        Assert.Equal(preco, entity.Preco);
        Assert.Equal(tipo, entity.Tipo);
        Assert.Equal(quantidade, entity.Quantidade);
        Assert.Equal(ativo, entity.Ativo);
        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.True((DateTime.UtcNow - entity.DataCadastro).TotalSeconds < 5); // DataCadastro recente
    }

    [Fact]
    public void Criar_DeveGerarNovoIdParaCadaInstancia() {
        // Arrange
        var nome = "Jogo Teste";
        var descricao = "Descrição do jogo";
        decimal preco = 99.99m;
        var tipo = "Aventura";
        long quantidade = 10;
        bool ativo = true;

        // Act
        var entity1 = JogosEntity.Criar(nome, descricao, preco, tipo, quantidade, ativo);
        var entity2 = JogosEntity.Criar(nome, descricao, preco, tipo, quantidade, ativo);

        // Assert
        Assert.NotEqual(entity1.Id, entity2.Id);
    }
}
