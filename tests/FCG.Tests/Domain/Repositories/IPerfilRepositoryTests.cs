using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Shared.Enum;
using Moq;

namespace FCG.Tests.Domain.Repositories;

public class IPerfilRepositoryTests
{
    [Fact]
    public async Task CriarPerfil_DeveSerChamadoComEntidadeCorreta() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var perfil = PerfilEntity.Criar("Usuário Teste", "usuario@teste.com", "senha123", PerfilEnum.Usuario);

        // Act
        await mockRepo.Object.CriarPerfil(perfil);

        // Assert
        mockRepo.Verify(r => r.CriarPerfil(perfil), Times.Once);
    }


    [Fact]
    public async Task TrazUsuario_DeveSerChamadoComEmailCorreto() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();
        var email = "usuario@teste.com";
        var perfil = PerfilEntity.Criar("Usuário Teste", email, "senha123", PerfilEnum.Usuario);
        mockRepo.Setup(r => r.TrazUsuario(email)).ReturnsAsync(perfil);

        // Act
        var result = await mockRepo.Object.TrazUsuario(email);

        // Assert
        mockRepo.Verify(r => r.TrazUsuario(email), Times.Once);
        Assert.Equal(perfil, result);
    }

    [Fact]
    public async Task Commit_DeveSerChamado() {
        // Arrange
        var mockRepo = new Mock<IPerfilRepository>();

        // Act
        await mockRepo.Object.Commit();

        // Assert
        mockRepo.Verify(r => r.Commit(), Times.Once);
    }
}
