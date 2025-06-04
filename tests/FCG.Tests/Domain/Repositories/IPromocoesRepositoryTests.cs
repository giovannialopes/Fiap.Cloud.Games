using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using Moq;

namespace FCG.Tests.Domain.Repositories;

public class IPromocoesRepositoryTests
{
    [Fact]
    public async Task AdicionarPromocoes_DeveSerChamadoComEntidadeCorreta() {
        // Arrange
        var mockRepo = new Mock<IPromocoesRepository>();
        var promocao = PromocoesEntity.Criar(
            "Promoção Teste",
            25.0m,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(5),
            new List<Guid> { Guid.NewGuid() }
        );

        // Act
        await mockRepo.Object.AdicionarPromocoes(promocao);

        // Assert
        mockRepo.Verify(r => r.AdicionarPromocoes(promocao), Times.Once);
    }

    [Fact]
    public async Task ConsultaPromocoesAtivas_DeveSerChamado() {
        // Arrange
        var mockRepo = new Mock<IPromocoesRepository>();
        var promocao = PromocoesEntity.Criar(
            "Promoção Ativa",
            10.0m,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(2),
            new List<Guid> { Guid.NewGuid() }
        );
        mockRepo.Setup(r => r.ConsultaPromocoesAtivas()).ReturnsAsync(promocao);

        // Act
        var result = await mockRepo.Object.ConsultaPromocoesAtivas();

        // Assert
        mockRepo.Verify(r => r.ConsultaPromocoesAtivas(), Times.Once);
        Assert.Equal(promocao, result);
    }

    [Fact]
    public async Task Commit_DeveSerChamado() {
        // Arrange
        var mockRepo = new Mock<IPromocoesRepository>();

        // Act
        await mockRepo.Object.Commit();

        // Assert
        mockRepo.Verify(r => r.Commit(), Times.Once);
    }
}
