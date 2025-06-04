using FCG.Domain.Entities;

namespace FCG.Tests.Domain.Entity;

public class BibliotecaEntityTests
{
    [Fact]
    public void Criar_DeveRetornarEntidadeComIdsCorretos() {
        // Arrange
        var jogoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        // Act
        var entity = BibliotecaEntity.Criar(jogoId, usuarioId);

        // Assert
        Assert.NotNull(entity);
        Assert.Equal(jogoId, entity.JogoId);
        Assert.Equal(usuarioId, entity.UsuarioId);
        Assert.NotEqual(Guid.Empty, entity.Id);
    }

    [Fact]
    public void Criar_DeveGerarNovoIdParaCadaInstancia() {
        // Arrange
        var jogoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        // Act
        var entity1 = BibliotecaEntity.Criar(jogoId, usuarioId);
        var entity2 = BibliotecaEntity.Criar(jogoId, usuarioId);

        // Assert
        Assert.NotEqual(entity1.Id, entity2.Id);
    }
}
