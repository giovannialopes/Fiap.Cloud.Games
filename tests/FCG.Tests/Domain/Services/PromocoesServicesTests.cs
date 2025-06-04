using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Class;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Moq;

namespace FCG.Tests.Domain.Services;

public class PromocoesServicesTests
{
    [Fact]
    public async Task InserePromocoes_DeveRetornarSucesso() {
        // Arrange
        var mockRepo = new Mock<IPromocoesRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new PromocoesDto.PromocoesDtoRequest {
            Nome = "Promoção Teste",
            Valor = 20m,
            DataInicio = DateTime.UtcNow,
            DataFim = DateTime.UtcNow.AddDays(5),
            IdJogos = new List<Guid> { Guid.NewGuid() }
        };

        var service = new PromocoesServices(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.InserePromocoes(request);

        // Assert
        mockRepo.Verify(r => r.AdicionarPromocoes(It.IsAny<PromocoesEntity>()), Times.Once);
        mockLogger.Verify(l => l.LogInformation("Finalizou InserePromocoes"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Nome, result.Value.Nome);
        Assert.Equal(request.Valor, result.Value.Valor);
        Assert.Equal(request.DataInicio, result.Value.DataInicio);
        Assert.Equal(request.DataFim, result.Value.DataFim);
        Assert.Equal(request.IdJogos, result.Value.IdJogos);
    }

    [Fact]
    public async Task ConsultaPromocoesAtivas_DeveRetornarSucesso_QuandoPromocaoExiste() {
        // Arrange
        var mockRepo = new Mock<IPromocoesRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var entity = PromocoesEntity.Criar(
            "Promoção Ativa",
            15m,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(2),
            new List<Guid> { Guid.NewGuid() }
        );
        mockRepo.Setup(r => r.ConsultaPromocoesAtivas()).ReturnsAsync(entity);

        var service = new PromocoesServices(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.ConsultaPromocoesAtivas();

        // Assert
        mockLogger.Verify(l => l.LogInformation("Finalizou ConsultaPromocoesAtivas"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(entity.Nome, result.Value.Nome);
        Assert.Equal(entity.Valor, result.Value.Valor);
        Assert.Equal(entity.DataInicio, result.Value.DataInicio);
        Assert.Equal(entity.DataFim, result.Value.DataFim);
        Assert.Equal(entity.IdJogos, result.Value.IdJogos);
    }

    [Fact]
    public async Task ConsultaPromocoesAtivas_DeveRetornarFalha_QuandoNaoExistePromocao() {
        // Arrange
        var mockRepo = new Mock<IPromocoesRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        mockRepo.Setup(r => r.ConsultaPromocoesAtivas()).ReturnsAsync((PromocoesEntity?)null);

        var service = new PromocoesServices(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.ConsultaPromocoesAtivas();

        // Assert
        mockLogger.Verify(l => l.LogError("Nenhuma promoção ativa encontrada."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Nenhuma promoção ativa encontrada.", result.Error.Message);
    }
}