using FCG.Api.Controllers;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FCG.Tests.API;

public class LoginControllerTests
{
    private static LoginController CreateController(Mock<ILoginServices> mockService, Mock<ILoggerServices>? mockLogger = null) {
        var logger = mockLogger ?? new Mock<ILoggerServices>();
        return new LoginController(mockService.Object, logger.Object);
    }

    [Fact]
    public async Task EntrarNoSistema_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<ILoginServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new LoginDto.LoginDtoRequest { Email = "teste@teste.com", SenhaHash = "senha123" };
        var response = new LoginDto.LoginDtoResponse { Id = Guid.NewGuid(), Token = "token-jwt" };
        mockService.Setup(s => s.Entrar(request)).ReturnsAsync(Result.Success(response));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.EntrarNoSistema(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task EntrarNoSistema_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<ILoginServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new LoginDto.LoginDtoRequest { Email = "teste@teste.com", SenhaHash = "senha123" };
        mockService.Setup(s => s.Entrar(request))
            .ReturnsAsync(Result.Failure<LoginDto.LoginDtoResponse>("Credenciais inválidas", "401"));
        var controller = CreateController(mockService, mockLogger);

        // Act
        var result = await controller.EntrarNoSistema(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Credenciais inválidas", ((dynamic)badRequest.Value).Message);
    }
}