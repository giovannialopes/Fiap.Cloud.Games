using FCG.Domain.Shared.DTO;

namespace FCG.Tests.Domain.Shared.DTO;

public class LoginDtoTests
{
    [Fact]
    public void LoginDtoRequest_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var email = "usuario@teste.com";
        var senhaHash = "hash123";

        // Act
        var dto = new LoginDto.LoginDtoRequest {
            Email = email,
            SenhaHash = senhaHash
        };

        // Assert
        Assert.Equal(email, dto.Email);
        Assert.Equal(senhaHash, dto.SenhaHash);
    }

    [Fact]
    public void LoginDtoRequest_ValoresPadraoDevemSerStringVazia() {
        // Act
        var dto = new LoginDto.LoginDtoRequest();

        // Assert
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.SenhaHash);
    }

    [Fact]
    public void LoginDtoResponse_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var id = Guid.NewGuid();
        var token = "token-jwt";

        // Act
        var dto = new LoginDto.LoginDtoResponse {
            Id = id,
            Token = token
        };

        // Assert
        Assert.Equal(id, dto.Id);
        Assert.Equal(token, dto.Token);
    }

    [Fact]
    public void LoginDtoResponse_ValorPadraoTokenDeveSerStringVazia() {
        // Act
        var dto = new LoginDto.LoginDtoResponse();

        // Assert
        Assert.Equal(string.Empty, dto.Token);
    }
}
