using FCG.Domain.Entities;

namespace FCG.Tests.Domain.Entity;

public class CarteiraEntityTests
{
    [Fact]
    public void Criar_DeveRetornarEntidadeComValoresCorretos() {
        // Arrange
        var usuarioId = Guid.NewGuid();
        decimal saldo = 150.75m;

        // Act
        var entity = CarteiraEntity.Criar(usuarioId, saldo);

        // Assert
        Assert.NotNull(entity);
        Assert.Equal(usuarioId, entity.UsuarioId);
        Assert.Equal(saldo, entity.Saldo);
        Assert.NotEqual(Guid.Empty, entity.Id);
    }

    [Fact]
    public void Criar_DeveGerarNovoIdParaCadaInstancia() {
        // Arrange
        var usuarioId = Guid.NewGuid();
        decimal saldo = 100m;

        // Act
        var entity1 = CarteiraEntity.Criar(usuarioId, saldo);
        var entity2 = CarteiraEntity.Criar(usuarioId, saldo);

        // Assert
        Assert.NotEqual(entity1.Id, entity2.Id);
    }

    [Fact]
    public void Atualizar_DeveRetornarEntidadeComValoresAtualizados() {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        decimal saldo = 200m;

        // Act
        var entity = CarteiraEntity.Atualizar(id, usuarioId, saldo);

        // Assert
        Assert.NotNull(entity);
        Assert.Equal(id, entity.Id);
        Assert.Equal(usuarioId, entity.UsuarioId);
        Assert.Equal(saldo, entity.Saldo);
    }
}
