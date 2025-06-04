using FCG.Domain.Shared.DTO;

namespace FCG.Tests.Domain.Shared.DTO;

public class PerfilDtoTests
{
    [Fact]
    public void PerfilRequest_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var nome = "Usuário Teste";
        var email = "usuario@teste.com";
        var senhaHash = "hash123";

        // Act
        var dto = new PerfilDto.PerfilRequest {
            Nome = nome,
            Email = email,
            SenhaHash = senhaHash
        };

        // Assert
        Assert.Equal(nome, dto.Nome);
        Assert.Equal(email, dto.Email);
        Assert.Equal(senhaHash, dto.SenhaHash);
    }

    [Fact]
    public void PerfilRequest_ValoresPadraoDevemSerStringVazia() {
        // Act
        var dto = new PerfilDto.PerfilRequest();

        // Assert
        Assert.Equal(string.Empty, dto.Nome);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.SenhaHash);
    }

    [Fact]
    public void PerfilResponse_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var id = Guid.NewGuid();
        var token = "token-jwt";

        // Act
        var dto = new PerfilDto.PerfilResponse {
            Id = id,
            Token = token
        };

        // Assert
        Assert.Equal(id, dto.Id);
        Assert.Equal(token, dto.Token);
    }

    [Fact]
    public void PerfilResponse_ValorPadraoTokenDeveSerStringVazia() {
        // Act
        var dto = new PerfilDto.PerfilResponse();

        // Assert
        Assert.Equal(string.Empty, dto.Token);
    }
}
