using FCG.Domain.Shared.DTO;

namespace FCG.Tests.Domain.Shared.DTO;

public class ErrorDtoTests
{
    [Fact]
    public void ErrorResponse_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var statusCode = 404;
        var message = "Recurso não encontrado.";

        // Act
        var error = new ErrorDto.ErrorResponse {
            StatusCode = statusCode,
            Message = message
        };

        // Assert
        Assert.Equal(statusCode, error.StatusCode);
        Assert.Equal(message, error.Message);
    }

    [Fact]
    public void ErrorResponse_Message_DeveSerStringVaziaPorPadrao() {
        // Act
        var error = new ErrorDto.ErrorResponse();

        // Assert
        Assert.Equal(string.Empty, error.Message);
    }
}
