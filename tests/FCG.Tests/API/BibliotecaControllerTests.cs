using FCG.Api.Controllers;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace FCG.Tests.API;

public class BibliotecaControllerTests
{
    [Fact]
    public async Task ConsultarBiblioteca_DeveRetornarOk_QuandoSucesso() {
        // Arrange
        var mockService = new Mock<IBibliotecaServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var userId = "usuario123";
        var response = new List<JogosDto.JogosDtoResponse>
        {
            new JogosDto.JogosDtoResponse { Nome = "Jogo 1" }
        };
        mockService.Setup(s => s.ConsultarBibliotecaPorUsuario(userId))
            .ReturnsAsync(Result.Success(response));

        var controller = new BibliotecaController(mockService.Object, mockLogger.Object);
        controller.ControllerContext = new ControllerContext {
            HttpContext = new DefaultHttpContext {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }))
            }
        };

        // Act
        var result = await controller.ConsultarBiblioteca();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task ConsultarBiblioteca_DeveRetornarBadRequest_QuandoFalha() {
        // Arrange
        var mockService = new Mock<IBibliotecaServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var userId = "usuario123";
        var errorMessage = "Não encontrada";
        mockService.Setup(s => s.ConsultarBibliotecaPorUsuario(userId))
            .ReturnsAsync(Result.Failure<List<JogosDto.JogosDtoResponse>>(errorMessage, "404"));

        var controller = new BibliotecaController(mockService.Object, mockLogger.Object);
        controller.ControllerContext = new ControllerContext {
            HttpContext = new DefaultHttpContext {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }))
            }
        };

        // Act
        var result = await controller.ConsultarBiblioteca();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errorMessage, ((dynamic)badRequestResult.Value).Message);
    }


}