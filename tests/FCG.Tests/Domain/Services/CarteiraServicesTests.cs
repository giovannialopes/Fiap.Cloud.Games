using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Class;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Moq;

namespace FCG.Tests.Domain.Services;

public class CarteiraServicesTests
{
    [Fact]
    public async Task ConsultaSaldos_DeveRetornarSucesso_QuandoCarteiraExiste() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        var carteira = CarteiraEntity.Criar(usuarioId, 100m);
        mockRepo.Setup(r => r.ObtemSaldoPorId(usuarioId)).ReturnsAsync(carteira);

        var service = new CarteiraServices(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.ConsultaSaldos(usuarioId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(100m, result.Value.Saldo);
        mockLogger.Verify(l => l.LogInformation("Finalizou ConsultaSaldos"), Times.Once);
    }

    [Fact]
    public async Task ConsultaSaldos_DeveRetornarFalha_QuandoCarteiraNaoExiste() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        mockRepo.Setup(r => r.ObtemSaldoPorId(usuarioId)).ReturnsAsync((CarteiraEntity?)null);

        var service = new CarteiraServices(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.ConsultaSaldos(usuarioId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Saldo não encontrado.", result.Error.Message);
        mockLogger.Verify(l => l.LogError("Saldo não encontrado."), Times.Once);
    }

    [Fact]
    public async Task InsereSaldos_DeveAtualizarSaldo_QuandoCarteiraExiste() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        var carteira = CarteiraEntity.Criar(usuarioId, 50m);
        mockRepo.Setup(r => r.ObtemSaldoPorId(usuarioId)).ReturnsAsync(carteira);

        var service = new CarteiraServices(mockRepo.Object, mockLogger.Object);
        var request = new CarteiraDto.CarteiraDtoRequest { UsuarioId = usuarioId, Saldo = 25m };

        // Act
        var result = await service.InsereSaldos(request);

        // Assert
        mockRepo.Verify(r => r.AlteraSaldo(It.Is<CarteiraEntity>(c => c.Saldo == 75m)), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(75m, result.Value.Saldo);
    }

    [Fact]
    public async Task InsereSaldos_DeveCriarNovaCarteira_QuandoCarteiraNaoExiste() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        mockRepo.Setup(r => r.ObtemSaldoPorId(usuarioId)).ReturnsAsync((CarteiraEntity?)null);

        var service = new CarteiraServices(mockRepo.Object, mockLogger.Object);
        var request = new CarteiraDto.CarteiraDtoRequest { UsuarioId = usuarioId, Saldo = 40m };

        // Act
        var result = await service.InsereSaldos(request);

        // Assert
        mockRepo.Verify(r => r.AdicionaSaldo(It.Is<CarteiraEntity>(c => c.UsuarioId == usuarioId && c.Saldo == 40m)), Times.Once);
        mockRepo.Verify(r => r.Commit(), Times.Once);
        mockLogger.Verify(l => l.LogInformation("Finalizou InserirSaldos"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(40m, result.Value.Saldo);
    }

    [Fact]
    public async Task RemoveSaldos_DeveZerarSaldo() {
        // Arrange
        var mockRepo = new Mock<ICarteiraRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        var carteira = CarteiraEntity.Criar(usuarioId, 200m);
        mockRepo.Setup(r => r.ObtemSaldoPorId(usuarioId)).ReturnsAsync(carteira);

        var service = new CarteiraServices(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.RemoveSaldos(usuarioId);

        // Assert
        mockRepo.Verify(r => r.AlteraSaldo(It.Is<CarteiraEntity>(c => c.Saldo == 0)), Times.Once);
        mockLogger.Verify(l => l.LogInformation("Finalizou DeletarSaldos"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Value.Saldo);
    }
}