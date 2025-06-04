using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using Moq;

namespace FCG.Tests.Domain.Repositories;

public class BibliotecaRepositoryTests
{
    [Fact]
    public async Task AdicionaNaBiblioteca_DeveSerChamadoComEntidadeCorreta() {
        // Arrange
        var mockRepo = new Mock<IBibliotecaRepository>();
        var entity = BibliotecaEntity.Criar(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await mockRepo.Object.AdicionaNaBiblioteca(entity);

        // Assert
        mockRepo.Verify(r => r.AdicionaNaBiblioteca(entity), Times.Once);
    }

    [Fact]
    public async Task Commit_DeveSerChamado() {
        // Arrange
        var mockRepo = new Mock<IBibliotecaRepository>();

        // Act
        await mockRepo.Object.Commit();

        // Assert
        mockRepo.Verify(r => r.Commit(), Times.Once);
    }
}
