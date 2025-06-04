using FCG.Domain.Shared.DTO;

namespace FCG.Tests.Domain.Shared.DTO;

public class CarteiraDtoTests
{
    [Fact]
    public void CarteiraDtoRequest_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var usuarioId = Guid.NewGuid();
        decimal saldo = 123.45m;

        // Act
        var dto = new CarteiraDto.CarteiraDtoRequest {
            UsuarioId = usuarioId,
            Saldo = saldo
        };

        // Assert
        Assert.Equal(usuarioId, dto.UsuarioId);
        Assert.Equal(saldo, dto.Saldo);
    }

    [Fact]
    public void CarteiraDtoResponse_DevePermitirSetarEObterSaldo() {
        // Arrange
        decimal saldo = 987.65m;

        // Act
        var dto = new CarteiraDto.CarteiraDtoResponse {
            Saldo = saldo
        };

        // Assert
        Assert.Equal(saldo, dto.Saldo);
    }
}
