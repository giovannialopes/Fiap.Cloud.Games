using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Class;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Moq;

namespace FCG.Tests.Domain.Services;

public class BibliotecaServicesTests
{
    [Fact]
    public async Task ConsultarBibliotecaPorUsuario_DeveRetornarSucesso_QuandoUsuarioPossuiJogos() {
        // Arrange
        var mockRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = "usuario123";
        var jogos = new List<JogosEntity>
        {
            JogosEntity.Criar("Jogo 1", "Desc 1", 10m, "Ação", 1, true),
            JogosEntity.Criar("Jogo 2", "Desc 2", 20m, "Aventura", 2, true)
        };

        mockRepo.Setup(r => r.ObterJogosPorUsuario(usuarioId)).ReturnsAsync(jogos);

        var service = new BibliotecaServices(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.ConsultarBibliotecaPorUsuario(usuarioId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(jogos.Count, result.Value.Count);
        Assert.Equal(jogos[0].Nome, result.Value[0].Nome);
        Assert.Equal(jogos[1].Nome, result.Value[1].Nome);
        mockLogger.Verify(l => l.LogInformation("Finalizou ConsultarBiblioteca"), Times.Once);
    }

    [Fact]
    public async Task ConsultarBibliotecaPorUsuario_DeveRetornarFalha_QuandoUsuarioNaoPossuiJogos() {
        // Arrange
        var mockRepo = new Mock<IJogosRepository>();
        var mockLogger = new Mock<ILoggerServices>();
        var usuarioId = "usuario123";
        mockRepo.Setup(r => r.ObterJogosPorUsuario(usuarioId)).ReturnsAsync(new List<JogosEntity>());

        var service = new BibliotecaServices(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.ConsultarBibliotecaPorUsuario(usuarioId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Usuário não possui jogos na biblioteca.", result.Error.Message);
        mockLogger.Verify(l => l.LogError($"Usuário {usuarioId} não possui jogos na biblioteca."), Times.Once);
    }
}