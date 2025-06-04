using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using Moq;

namespace FCG.Tests.Domain.Repositories;

public class IJogosRepositoryTests
{
    [Fact]
    public async Task AdicionarJogos_DeveSerChamadoComEntidadeCorreta() {
        // Arrange
        var mockRepo = new Mock<IJogosRepository>();
        var jogo = JogosEntity.Criar("Jogo Teste", "Descrição", 99.99m, "Aventura", 10, true);

        // Act
        await mockRepo.Object.AdicionarJogos(jogo);

        // Assert
        mockRepo.Verify(r => r.AdicionarJogos(jogo), Times.Once);
    }

    [Fact]
    public async Task AtualizarJogos_DeveSerChamadoComEntidadeCorreta() {
        // Arrange
        var mockRepo = new Mock<IJogosRepository>();
        var jogo = JogosEntity.Criar("Jogo Atualizado", "Nova Descrição", 59.99m, "RPG", 5, true);

        // Act
        await mockRepo.Object.AtualizarJogos(jogo);

        // Assert
        mockRepo.Verify(r => r.AtualizarJogos(jogo), Times.Once);
    }

    [Fact]
    public async Task ObterJogoPorNome_DeveSerChamadoComNomeCorreto() {
        // Arrange
        var mockRepo = new Mock<IJogosRepository>();
        var nome = "Jogo Teste";
        var jogo = JogosEntity.Criar(nome, "Descrição", 99.99m, "Aventura", 10, true);
        mockRepo.Setup(r => r.ObterJogoPorNome(nome)).ReturnsAsync(jogo);

        // Act
        var result = await mockRepo.Object.ObterJogoPorNome(nome);

        // Assert
        mockRepo.Verify(r => r.ObterJogoPorNome(nome), Times.Once);
        Assert.Equal(jogo, result);
    }

    [Fact]
    public async Task Commit_DeveSerChamado() {
        // Arrange
        var mockRepo = new Mock<IJogosRepository>();

        // Act
        await mockRepo.Object.Commit();

        // Assert
        mockRepo.Verify(r => r.Commit(), Times.Once);
    }
}
