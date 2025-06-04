using FCG.Api.Controllers;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FCG.Tests.API;

public class PerfilControllerTests
{
    private static PerfilController CreateController(Mock<IPerfilServices> mockService, Mock<ILoggerServices>? mockLogger = null) {
        var logger = mockLogger ?? new Mock<ILoggerServices>();
        return new PerfilController(mockService.Object, logger.Object);
    }

    [Fact]
    public async Task CriarPerfil_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IPerfilServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new PerfilDto.PerfilRequest { Nome = "Usuário", Email = "usuario@teste.com", SenhaHash = "senha123" };
        var response = new PerfilDto.PerfilResponse { Id = Guid.NewGuid(), Token = "token-jwt" };
        mockService.Setup(s => s.CriarPerfil(request)).ReturnsAsync(Result.Success(response));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.CriarPerfil(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task CriarPerfil_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IPerfilServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new PerfilDto.PerfilRequest { Nome = "Usuário", Email = "usuario@teste.com", SenhaHash = "senha123" };
        mockService.Setup(s => s.CriarPerfil(request))
            .ReturnsAsync(Result.Failure<PerfilDto.PerfilResponse>("Erro ao criar usuário", "400"));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.CriarPerfil(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro ao criar usuário", ((dynamic)badRequest.Value).Message);
    }
}