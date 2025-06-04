using FCG.Api.Controllers;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FCG.Tests.API;

public class CarteiraControllerTests
{
    [Fact]
    public async Task InserirSaldos_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<ICarteiraServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new CarteiraDto.CarteiraDtoRequest { UsuarioId = Guid.NewGuid(), Saldo = 100 };
        var response = new CarteiraDto.CarteiraDtoResponse { Saldo = 100 };
        mockService.Setup(s => s.InsereSaldos(request))
            .ReturnsAsync(Result.Success(response));

        var controller = new CarteiraController(mockService.Object, mockLogger.Object);

        // Act
        var result = await controller.InserirSaldos(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task InserirSaldos_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<ICarteiraServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new CarteiraDto.CarteiraDtoRequest { UsuarioId = Guid.NewGuid(), Saldo = 100 };
        mockService.Setup(s => s.InsereSaldos(request))
            .ReturnsAsync(Result.Failure<CarteiraDto.CarteiraDtoResponse>("Erro ao inserir", "400"));

        var controller = new CarteiraController(mockService.Object, mockLogger.Object);

        // Act
        var result = await controller.InserirSaldos(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro ao inserir", ((dynamic)badRequestResult.Value).Message);
    }

    [Fact]
    public async Task ConsultaSaldos_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<ICarteiraServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        var response = new CarteiraDto.CarteiraDtoResponse { Saldo = 200 };
        mockService.Setup(s => s.ConsultaSaldos(usuarioId))
            .ReturnsAsync(Result.Success(response));

        var controller = new CarteiraController(mockService.Object, mockLogger.Object);

        // Act
        var result = await controller.ConsultaSaldos(usuarioId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task ConsultaSaldos_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<ICarteiraServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        mockService.Setup(s => s.ConsultaSaldos(usuarioId))
            .ReturnsAsync(Result.Failure<CarteiraDto.CarteiraDtoResponse>("Não encontrado", "404"));

        var controller = new CarteiraController(mockService.Object, mockLogger.Object);

        // Act
        var result = await controller.ConsultaSaldos(usuarioId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Não encontrado", ((dynamic)badRequestResult.Value).Message);
    }

    [Fact]
    public async Task DeletarSaldos_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<ICarteiraServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        var response = new CarteiraDto.CarteiraDtoResponse { Saldo = 0 };
        mockService.Setup(s => s.RemoveSaldos(usuarioId))
            .ReturnsAsync(Result.Success(response));

        var controller = new CarteiraController(mockService.Object, mockLogger.Object);

        // Act
        var result = await controller.DeletarSaldos(usuarioId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task DeletarSaldos_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<ICarteiraServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = Guid.NewGuid();
        mockService.Setup(s => s.RemoveSaldos(usuarioId))
            .ReturnsAsync(Result.Failure<CarteiraDto.CarteiraDtoResponse>("Erro ao remover", "400"));

        var controller = new CarteiraController(mockService.Object, mockLogger.Object);

        // Act
        var result = await controller.DeletarSaldos(usuarioId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro ao remover", ((dynamic)badRequestResult.Value).Message);
    }
}