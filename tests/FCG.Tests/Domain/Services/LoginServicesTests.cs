using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Class;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Enum;
using FCG.Domain.Shared.Results;
using Moq;

namespace FCG.Tests.Domain.Services;

public class LoginServicesTests
{
    [Fact]
    public async Task Entrar_DeveRetornarFalha_QuandoEmailOuSenhaNaoInformados() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var service = new LoginServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.Entrar(new LoginDto.LoginDtoRequest { Email = "", SenhaHash = "" });

        // Assert
        mockLogger.Verify(l => l.LogError("Email ou senha não informados."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Email ou senha não informados.", result.Error.Message);
    }

    [Fact]
    public async Task Entrar_DeveRetornarFalha_QuandoUsuarioNaoEncontrado() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        mockRepo.Setup(r => r.TrazUsuario("usuario@teste.com")).ReturnsAsync((PerfilEntity?)null);

        var service = new LoginServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.Entrar(new LoginDto.LoginDtoRequest { Email = "usuario@teste.com", SenhaHash = "senha123" });

        // Assert
        mockLogger.Verify(l => l.LogError("Usuário não encontrado."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Usuário não encontrado.", result.Error.Message);
    }

    [Fact]
    public async Task Entrar_DeveRetornarFalha_QuandoSenhaInvalida() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuario = PerfilEntity.Criar("Usuário", "usuario@teste.com", "senhaCorreta", PerfilEnum.Usuario);
        mockRepo.Setup(r => r.TrazUsuario("usuario@teste.com")).ReturnsAsync(usuario);

        var service = new LoginServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.Entrar(new LoginDto.LoginDtoRequest { Email = "usuario@teste.com", SenhaHash = "senhaErrada" });

        // Assert
        mockLogger.Verify(l => l.LogError("Senha inválida."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Senha inválida.", result.Error.Message);
    }

    [Fact]
    public async Task Entrar_DeveRetornarSucesso_QuandoLoginValido() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var senha = "senha123";
        var usuario = PerfilEntity.Criar("Usuário", "usuario@teste.com", senha, PerfilEnum.Administrador);
        mockRepo.Setup(r => r.TrazUsuario("usuario@teste.com")).ReturnsAsync(usuario);
        mockJwt.Setup(j => j.GenerateToken(usuario.Id, PerfilEnum.Administrador.ToString())).Returns("token-jwt");

        var service = new LoginServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.Entrar(new LoginDto.LoginDtoRequest { Email = "usuario@teste.com", SenhaHash = senha });

        // Assert
        mockLogger.Verify(l => l.LogInformation("Finalizou EntrarNoSistema"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(usuario.Id, result.Value.Id);
        Assert.Equal("token-jwt", result.Value.Token);
    }
}