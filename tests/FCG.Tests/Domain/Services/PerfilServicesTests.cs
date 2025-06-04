using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Class;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Enum;
using FCG.Domain.Shared.Results;
using Moq;

namespace FCG.Tests.Domain.Services;

public class PerfilServicesTests
{
    [Fact]
    public async Task CriarPerfil_DeveRetornarSucesso() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new PerfilDto.PerfilRequest {
            Nome = "Usuário",
            Email = "usuario@teste.com",
            SenhaHash = "senha123",
            PerfilEnum = PerfilEnum.Usuario
        };
        mockJwt.Setup(j => j.GenerateToken(It.IsAny<Guid>(), PerfilEnum.Usuario.ToString())).Returns("token-usuario");

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.CriarPerfil(request);

        // Assert
        mockRepo.Verify(r => r.CriarPerfil(It.Is<PerfilEntity>(p => p.Email == request.Email && p.Perfil == PerfilEnum.Usuario)), Times.Once);
        mockRepo.Verify(r => r.Commit(), Times.Once);
        mockLogger.Verify(l => l.LogInformation("Finalizou CriarPerfil"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal("token-usuario", result.Value.Token);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Fact]
    public async Task CriarPerfil_DeveRetornarFalha_QuandoRepositoryLancaExcecao() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new PerfilDto.PerfilRequest {
            Nome = "Usuário",
            Email = "usuario@teste.com",
            SenhaHash = "senha123",
            PerfilEnum = PerfilEnum.Usuario
        };
        mockRepo.Setup(r => r.CriarPerfil(It.IsAny<PerfilEntity>())).ThrowsAsync(new Exception("erro"));

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.CriarPerfil(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Erro ao criar perfil usuário", result.Error.Message);
    }

    [Fact]
    public async Task AlterarPerfil_DeveRetornarSucesso() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var email = "usuario@teste.com";
        var senha = "senha123";
        var usuario = PerfilEntity.Criar("Usuário", email, senha, PerfilEnum.Usuario);
        mockRepo.Setup(r => r.TrazUsuario(email)).ReturnsAsync(usuario);
        var request = new PerfilDto.PerfilRequest {
            Nome = "Novo Nome",
            Email = "novoemail@teste.com",
            SenhaHash = "novaSenha",
            PerfilEnum = PerfilEnum.Usuario
        };

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.AlterarPerfil(email, senha, request);

        // Assert
        mockRepo.Verify(r => r.AtualizaPerfil(It.Is<PerfilEntity>(p => p.Email == request.Email && p.Nome == request.Nome)), Times.Once);
        mockLogger.Verify(l => l.LogInformation("Finalizou AlterarPerfil"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal("Perfil atualizado com sucesso.", result.Value);
    }

    [Fact]
    public async Task AlterarPerfil_DeveRetornarFalha_QuandoEmailOuSenhaVazios() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new PerfilDto.PerfilRequest {
            Nome = "Novo Nome",
            Email = "novoemail@teste.com",
            SenhaHash = "novaSenha",
            PerfilEnum = PerfilEnum.Usuario
        };

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.AlterarPerfil("", "", request);

        // Assert
        mockLogger.Verify(l => l.LogError("Email ou senha não informados."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Email ou senha não informados.", result.Error.Message);
    }

    [Fact]
    public async Task AlterarPerfil_DeveRetornarFalha_QuandoUsuarioNaoEncontrado() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var email = "usuario@teste.com";
        var senha = "senha123";
        mockRepo.Setup(r => r.TrazUsuario(email)).ReturnsAsync((PerfilEntity?)null);
        var request = new PerfilDto.PerfilRequest {
            Nome = "Novo Nome",
            Email = "novoemail@teste.com",
            SenhaHash = "novaSenha",
            PerfilEnum = PerfilEnum.Usuario
        };

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.AlterarPerfil(email, senha, request);

        // Assert
        mockLogger.Verify(l => l.LogError("Usuário não encontrado para alteração."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Usuário não encontrado para alteração.", result.Error.Message);
    }

    [Fact]
    public async Task AlterarPerfil_DeveRetornarFalha_QuandoSenhaInvalida() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var email = "usuario@teste.com";
        var senha = "senhaErrada";
        var usuario = PerfilEntity.Criar("Usuário", email, "senhaCorreta", PerfilEnum.Usuario);
        mockRepo.Setup(r => r.TrazUsuario(email)).ReturnsAsync(usuario);
        var request = new PerfilDto.PerfilRequest {
            Nome = "Novo Nome",
            Email = "novoemail@teste.com",
            SenhaHash = "novaSenha",
            PerfilEnum = PerfilEnum.Usuario
        };

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.AlterarPerfil(email, senha, request);

        // Assert
        mockLogger.Verify(l => l.LogError("Senha inválida para alteração."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Senha inválida para alteração.", result.Error.Message);
    }

    [Fact]
    public async Task DeletarPerfil_DeveRetornarSucesso() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var email = "usuario@teste.com";
        var senha = "senha123";
        var usuario = PerfilEntity.Criar("Usuário", email, senha, PerfilEnum.Usuario);
        mockRepo.Setup(r => r.TrazUsuario(email)).ReturnsAsync(usuario);

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.DeletarPerfil(email, senha);

        // Assert
        mockRepo.Verify(r => r.AtualizaPerfil(It.Is<PerfilEntity>(p => p.Habilitado == 0)), Times.Once);
        mockLogger.Verify(l => l.LogInformation("Finalizou DeletarPerfil"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal("Perfil desativado com sucesso.", result.Value);
    }

    [Fact]
    public async Task DeletarPerfil_DeveRetornarFalha_QuandoEmailOuSenhaVazios() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.DeletarPerfil("", "");

        // Assert
        mockLogger.Verify(l => l.LogError("Email ou senha não informados."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Email ou senha não informados.", result.Error.Message);
    }

    [Fact]
    public async Task DeletarPerfil_DeveRetornarFalha_QuandoUsuarioNaoEncontrado() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var email = "usuario@teste.com";
        var senha = "senha123";
        mockRepo.Setup(r => r.TrazUsuario(email)).ReturnsAsync((PerfilEntity?)null);

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.DeletarPerfil(email, senha);

        // Assert
        mockLogger.Verify(l => l.LogError("Usuário não encontrado para alteração."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Usuário não encontrado para alteração.", result.Error.Message);
    }

    [Fact]
    public async Task DeletarPerfil_DeveRetornarFalha_QuandoSenhaInvalida() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var mockJwt = new Mock<IJwtServices>();
        var mockLogger = new Mock<ILoggerServices>();
        var email = "usuario@teste.com";
        var senha = "senhaErrada";
        var usuario = PerfilEntity.Criar("Usuário", email, "senhaCorreta", PerfilEnum.Usuario);
        mockRepo.Setup(r => r.TrazUsuario(email)).ReturnsAsync(usuario);

        var service = new PerfilServices(mockRepo.Object, mockJwt.Object, mockLogger.Object);

        // Act
        var result = await service.DeletarPerfil(email, senha);

        // Assert
        mockLogger.Verify(l => l.LogError("Senha inválida para alteração."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Senha inválida para alteração.", result.Error.Message);
    }
}