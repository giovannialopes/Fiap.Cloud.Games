using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using Moq;

namespace FCG.Tests.Domain.Repositories;

public class CarteiraRepositoryTests
{
    [Fact]
    public async Task ObtemSaldoPorId_DeveSerChamadoComIdCorreto() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();
        var usuarioId = Guid.NewGuid();
        var carteira = CarteiraEntity.Criar(usuarioId, 100m);
        mockRepo.Setup(r => r.ObtemSaldoPorId(usuarioId)).ReturnsAsync(carteira);

        // Act
        var result = await mockRepo.Object.ObtemSaldoPorId(usuarioId);

        // Assert
        mockRepo.Verify(r => r.ObtemSaldoPorId(usuarioId), Times.Once);
        Assert.Equal(carteira, result);
    }

    [Fact]
    public async Task AdicionaSaldo_DeveSerChamadoComEntidadeCorreta() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();
        var carteira = CarteiraEntity.Criar(Guid.NewGuid(), 200m);

        // Act
        await mockRepo.Object.AdicionaSaldo(carteira);

        // Assert
        mockRepo.Verify(r => r.AdicionaSaldo(carteira), Times.Once);
    }

    [Fact]
    public void AlteraSaldo_DeveSerChamadoComEntidadeCorreta() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();
        var carteira = CarteiraEntity.Criar(Guid.NewGuid(), 300m);

        // Act
        mockRepo.Object.AlteraSaldo(carteira);

        // Assert
        mockRepo.Verify(r => r.AlteraSaldo(carteira), Times.Once);
    }

    [Fact]
    public async Task Commit_DeveSerChamado() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();

        // Act
        await mockRepo.Object.Commit();

        // Assert
        mockRepo.Verify(r => r.Commit(), Times.Once);
    }
}
