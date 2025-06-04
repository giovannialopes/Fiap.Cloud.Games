using FCG.Domain.Utils;
using Xunit;

namespace FCG.Tests.Domain.Utils;

public class ValidarSenhaTests
{
    [Fact]
    public void Validar_DeveRetornarTrue_QuandoSenhaCorreta() {
        // Arrange
        var senha = "senhaSegura123";
        var hash = BCrypt.Net.BCrypt.HashPassword(senha);

        // Act
        var resultado = ValidarSenha.Validar(senha, hash);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void Validar_DeveRetornarFalse_QuandoSenhaIncorreta() {
        // Arrange
        var senhaCorreta = "senhaSegura123";
        var senhaErrada = "outraSenha";
        var hash = BCrypt.Net.BCrypt.HashPassword(senhaCorreta);

        // Act
        var resultado = ValidarSenha.Validar(senhaErrada, hash);

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void Validar_DeveRetornarFalse_QuandoSenhaOuHashVazio() {
        // Arrange
        var hash = BCrypt.Net.BCrypt.HashPassword("qualquerSenha");

        // Act & Assert
        Assert.False(ValidarSenha.Validar("", hash));
        Assert.False(ValidarSenha.Validar("senha", ""));
        Assert.False(ValidarSenha.Validar(null, hash));
        Assert.False(ValidarSenha.Validar("senha", null));
    }
}
