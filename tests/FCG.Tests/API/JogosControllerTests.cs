using FCG.Api.Controllers;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace FCG.Tests.API;

public class JogosControllerTests
{
    private static JogosController CreateController(Mock<IJogosServices> mockService, Mock<ILoggerServices>? mockLogger = null, ClaimsPrincipal? user = null) {
        var logger = mockLogger ?? new Mock<ILoggerServices>();
        var controller = new JogosController(mockService.Object, logger.Object);
        if (user != null)
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        return controller;
    }

    [Fact]
    public async Task CadastrarJogos_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new JogosDto.JogosDtoRequest { Nome = "Jogo", Preco = 10, Descricao = "Desc", Quantidade = 1, Tipo = "Ação" };
        var response = new JogosDto.JogosDtoResponse { Nome = "Jogo" };
        mockService.Setup(s => s.CadastrarJogos(request)).ReturnsAsync(Result.Success(response));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.CadastrarJogos(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task CadastrarJogos_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new JogosDto.JogosDtoRequest { Nome = "Jogo", Preco = 10, Descricao = "Desc", Quantidade = 1, Tipo = "Ação" };
        mockService.Setup(s => s.CadastrarJogos(request)).ReturnsAsync(Result.Failure<JogosDto.JogosDtoResponse>("Erro", "400"));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.CadastrarJogos(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro", ((dynamic)badRequest.Value).Message);
    }

    [Fact]
    public async Task ComprarJogos_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new JogosDto.JogosDtoComprarJogos { Nome = "Jogo" };
        var response = new JogosDto.JogosDtoResponse { Nome = "Jogo" };
        var userId = Guid.NewGuid().ToString();
        mockService.Setup(s => s.ComprarJogos(request, userId)).ReturnsAsync(Result.Success(response));
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
        var controller = CreateController(mockService, mockLogger, user);

        // Act
        var result = await controller.ComprarJogos(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }


   [Fact]
    public async Task ComprarJogos_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new JogosDto.JogosDtoComprarJogos { Nome = "Jogo" };
        var userId = Guid.NewGuid().ToString();
        mockService.Setup(s => s.ComprarJogos(request, userId)).ReturnsAsync(Result.Failure<JogosDto.JogosDtoResponse>("Erro", "400"));
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
        var controller = CreateController(mockService, mockLogger, user);

        // Act
        var result = await controller.ComprarJogos(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro", ((dynamic)badRequest.Value).Message);
    }

    [Fact]
    public async Task ConsultarUnicoJogo_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var nome = "Jogo";
        var response = new JogosDto.JogosDtoResponse { Nome = nome };
        mockService.Setup(s => s.ConsultarUnicoJogo(nome)).ReturnsAsync(Result.Success(response));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.ConsultarUnicoJogo(nome);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task ConsultarUnicoJogo_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var nome = "Jogo";
        mockService.Setup(s => s.ConsultarUnicoJogo(nome)).ReturnsAsync(Result.Failure<JogosDto.JogosDtoResponse>("Erro", "404"));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.ConsultarUnicoJogo(nome);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro", ((dynamic)badRequest.Value).Message);
    }

    [Fact]
    public async Task ConsultarJogos_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var response = new List<JogosDto.JogosDtoResponse> { new JogosDto.JogosDtoResponse { Nome = "Jogo" } };
        mockService.Setup(s => s.ConsultarJogos()).ReturnsAsync(Result.Success(response));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.ConsultarJogos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task ConsultarJogos_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        mockService.Setup(s => s.ConsultarJogos()).ReturnsAsync(Result.Failure<List<JogosDto.JogosDtoResponse>>("Erro", "404"));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.ConsultarJogos();

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro", ((dynamic)badRequest.Value).Message);
    }

    [Fact]
    public async Task DeletarJogos_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var nome = "Jogo";
        var response = new JogosDto.JogosDtoResponse { Nome = nome };
        mockService.Setup(s => s.DeletarJogos(nome)).ReturnsAsync(Result.Success(response));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.DeletarJogos(nome);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task DeletarJogos_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IJogosServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var nome = "Jogo";
        mockService.Setup(s => s.DeletarJogos(nome)).ReturnsAsync(Result.Failure<JogosDto.JogosDtoResponse>("Erro", "400"));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.DeletarJogos(nome);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro", ((dynamic)badRequest.Value).Message);
    }
}