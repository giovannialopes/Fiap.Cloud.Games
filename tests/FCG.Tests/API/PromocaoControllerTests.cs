using FCG.Api.Controllers;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FCG.Tests.API;

public class PromocaoControllerTests
{
    private static PromocaoController CreateController(Mock<IPromocoesServices> mockService, Mock<ILoggerServices>? mockLogger = null)
        => new PromocaoController(mockService.Object, (mockLogger ?? new Mock<ILoggerServices>()).Object);

    [Fact]
    public async Task InserirPromocoes_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IPromocoesServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new PromocoesDto.PromocoesDtoRequest {
            Nome = "Promoção Teste",
            Valor = 10,
            DataInicio = DateTime.UtcNow,
            DataFim = DateTime.UtcNow.AddDays(5),
            IdJogos = new List<Guid> { Guid.NewGuid() }
        };
        var response = new PromocoesDto.PromocoesDtoResponse {
            Nome = "Promoção Teste",
            Valor = 10,
            DataInicio = request.DataInicio,
            DataFim = request.DataFim,
            IdJogos = request.IdJogos
        };
        mockService.Setup(s => s.InserePromocoes(request)).ReturnsAsync(Result.Success(response));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.InserirPromocoes(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task InserirPromocoes_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IPromocoesServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new PromocoesDto.PromocoesDtoRequest {
            Nome = "Promoção Teste",
            Valor = 10,
            DataInicio = DateTime.UtcNow,
            DataFim = DateTime.UtcNow.AddDays(5),
            IdJogos = new List<Guid> { Guid.NewGuid() }
        };
        mockService.Setup(s => s.InserePromocoes(request))
            .ReturnsAsync(Result.Failure<PromocoesDto.PromocoesDtoResponse>("Erro ao inserir promoção", "400"));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.InserirPromocoes(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro ao inserir promoção", ((dynamic)badRequest.Value).Message);
    }

    [Fact]
    public async Task ConsultarPromocoes_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IPromocoesServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var response = new PromocoesDto.PromocoesDtoResponse {
            Nome = "Promoção Ativa",
            Valor = 20,
            DataInicio = DateTime.UtcNow,
            DataFim = DateTime.UtcNow.AddDays(2),
            IdJogos = new List<Guid> { Guid.NewGuid() }
        };
        mockService.Setup(s => s.ConsultaPromocoesAtivas()).ReturnsAsync(Result.Success(response));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.ConsultarPromocoes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task ConsultarPromocoes_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IPromocoesServices>();
        var mockLogger = new Mock<ILoggerServices>();
        mockService.Setup(s => s.ConsultaPromocoesAtivas())
            .ReturnsAsync(Result.Failure<PromocoesDto.PromocoesDtoResponse>("Nenhuma promoção ativa encontrada", "404"));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.ConsultarPromocoes();

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Nenhuma promoção ativa encontrada", ((dynamic)badRequest.Value).Message);
    }
}