using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Class;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Moq;

namespace FCG.Tests.Domain.Services;

public class JogosServicesTests
{
    private static JogosServices CreateService(
        Mock<IJogosRepository>? mockJogosRepo = null,
        Mock<ICarteiraRepository>? mockCarteiraRepo = null,
        Mock<IBibliotecaRepository>? mockBibliotecaRepo = null,
        Mock<IPromocoesServices>? mockPromocoes = null,
        Mock<ILoggerServices>? mockLogger = null) {
        return new JogosServices(
            (mockJogosRepo ?? new Mock<IJogosRepository>()).Object,
            (mockCarteiraRepo ?? new Mock<ICarteiraRepository>()).Object,
            (mockBibliotecaRepo ?? new Mock<IBibliotecaRepository>()).Object,
            (mockPromocoes ?? new Mock<IPromocoesServices>()).Object,
            (mockLogger ?? new Mock<ILoggerServices>()).Object
        );
    }

    [Fact]
    public async Task CadastrarJogos_DeveRetornarSucesso() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new JogosDto.JogosDtoRequest {
            Nome = "Jogo Teste",
            Descricao = "Desc",
            Preco = 100m,
            Tipo = "Ação",
            Quantidade = 5
        };
        mockJogosRepo.Setup(r => r.ObterJogoPorNome(request.Nome)).ReturnsAsync((JogosEntity?)null);

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.CadastrarJogos(request);

        // Assert
        mockJogosRepo.Verify(r => r.AdicionarJogos(It.IsAny<JogosEntity>()), Times.Once);
        mockLogger.Verify(l => l.LogInformation("Finalizou CadastrarJogos"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Nome, result.Value.Nome);
    }

    [Fact]
    public async Task CadastrarJogos_DeveRetornarFalha_QuandoJogoJaExiste() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var request = new JogosDto.JogosDtoRequest {
            Nome = "Jogo Existente",
            Descricao = "Desc",
            Preco = 100m,
            Tipo = "Ação",
            Quantidade = 5
        };
        mockJogosRepo.Setup(r => r.ObterJogoPorNome(request.Nome)).ReturnsAsync(JogosEntity.Criar(request.Nome, "Desc", 100m, "Ação", 5, true));

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.CadastrarJogos(request);

        // Assert
        mockLogger.Verify(l => l.LogError("Jogo já foi cadastrado."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Esse jogo já foi cadastrado.", result.Error.Message);
    }

    [Fact]
    public async Task ComprarJogos_DeveRetornarFalha_QuandoTokenInvalido() {
        // Arrange
        var mockLogger = new Mock<ILoggerServices>();
        var service = CreateService(mockLogger: mockLogger);

        // Act
        var result = await service.ComprarJogos(new JogosDto.JogosDtoComprarJogos { Nome = "Jogo" }, "");

        // Assert
        mockLogger.Verify(l => l.LogError("Token ausente ou inválido."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Token ausente ou inválido.", result.Error.Message);
    }

    [Fact]
    public async Task ComprarJogos_DeveRetornarFalha_QuandoJogoNaoExiste() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        mockJogosRepo.Setup(r => r.ObterJogoPorNome("Inexistente")).ReturnsAsync((JogosEntity?)null);

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.ComprarJogos(new JogosDto.JogosDtoComprarJogos { Nome = "Inexistente" }, Guid.NewGuid().ToString());

        // Assert
        mockLogger.Verify(l => l.LogError("Jogo não encontrado."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Jogo não encontrado.", result.Error.Message);
    }

    [Fact]
    public async Task ComprarJogos_DeveRetornarFalha_QuandoJogoIndisponivel() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var jogo = JogosEntity.Criar("Jogo", "Desc", 100m, "Ação", 0, true);
        mockJogosRepo.Setup(r => r.ObterJogoPorNome("Jogo")).ReturnsAsync(jogo);

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.ComprarJogos(new JogosDto.JogosDtoComprarJogos { Nome = "Jogo" }, Guid.NewGuid().ToString());

        // Assert
        mockLogger.Verify(l => l.LogError("Esse jogo não está disponível."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Esse jogo não está disponível.", result.Error.Message);
    }


    [Fact]
    public async Task ConsultarJogos_DeveRetornarFalha_QuandoNaoExistemJogos() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        mockJogosRepo.Setup(r => r.ObterJogos()).ReturnsAsync(new List<JogosEntity>());

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.ConsultarJogos();

        // Assert
        mockLogger.Verify(l => l.LogError("Essa plataforma não possui jogos disponíveis."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Essa plataforma não possui jogos disponíveis.", result.Error.Message);
    }

    [Fact]
    public async Task ConsultarJogos_DeveRetornarSucesso_QuandoExistemJogos() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var jogos = new List<JogosEntity>
        {
            JogosEntity.Criar("Jogo 1", "Desc 1", 10m, "Ação", 1, true),
            JogosEntity.Criar("Jogo 2", "Desc 2", 20m, "Aventura", 2, true)
        };
        mockJogosRepo.Setup(r => r.ObterJogos()).ReturnsAsync(jogos);

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.ConsultarJogos();

        // Assert
        mockLogger.Verify(l => l.LogInformation("Finalizou ConsultarJogos"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal("Jogo 1", result.Value[0].Nome);
        Assert.Equal("Jogo 2", result.Value[1].Nome);
    }

    [Fact]
    public async Task ConsultarUnicoJogo_DeveRetornarSucesso_QuandoJogoExiste() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var jogo = JogosEntity.Criar("Jogo", "Desc", 100m, "Ação", 1, true);
        mockJogosRepo.Setup(r => r.ObterJogoPorNome("Jogo")).ReturnsAsync(jogo);

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.ConsultarUnicoJogo("Jogo");

        // Assert
        mockLogger.Verify(l => l.LogInformation("Finalizou ConsultarUnicoJogo"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal("Jogo", result.Value.Nome);
    }

    [Fact]
    public async Task ConsultarUnicoJogo_DeveRetornarFalha_QuandoJogoNaoExiste() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        mockJogosRepo.Setup(r => r.ObterJogoPorNome("Inexistente")).ReturnsAsync((JogosEntity?)null);

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.ConsultarUnicoJogo("Inexistente");

        // Assert
        mockLogger.Verify(l => l.LogError("Jogo não encontrado."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Jogo não encontrado.", result.Error.Message);
    }

    [Fact]
    public async Task DeletarJogos_DeveRetornarSucesso_QuandoJogoExiste() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var jogo = JogosEntity.Criar("Jogo", "Desc", 100m, "Ação", 1, true);
        mockJogosRepo.Setup(r => r.ObterJogoPorNome("Jogo")).ReturnsAsync(jogo);

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.DeletarJogos("Jogo");

        // Assert
        mockJogosRepo.Verify(r => r.AtualizarJogos(It.Is<JogosEntity>(j => j.Ativo == false)), Times.Once);
        mockLogger.Verify(l => l.LogInformation("Finalizou DeletarJogos"), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal("Jogo", result.Value.Nome);
    }

    [Fact]
    public async Task DeletarJogos_DeveRetornarFalha_QuandoJogoNaoExiste() {
        // Arrange
        var mockJogosRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        mockJogosRepo.Setup(r => r.ObterJogoPorNome("Inexistente")).ReturnsAsync((JogosEntity?)null);

        var service = CreateService(mockJogosRepo: mockJogosRepo, mockLogger: mockLogger);

        // Act
        var result = await service.DeletarJogos("Inexistente");

        // Assert
        mockLogger.Verify(l => l.LogError("Jogo não encontrado."), Times.Once);
        Assert.False(result.IsSuccess);
        Assert.Equal("Jogo não encontrado.", result.Error.Message);
    }
}