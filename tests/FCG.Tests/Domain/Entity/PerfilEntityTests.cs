using FCG.Domain.Entities;
using FCG.Domain.Shared.Enum;

namespace FCG.Tests.Domain.Entity;

public class PerfilEntityTests
{
    [Fact]
    public void Criar_DeveRetornarEntidadeComValoresCorretos() {
        // Arrange
        var nome = "Usuário Teste";
        var email = "usuario@teste.com";
        var senha = "senha123";
        var perfil = PerfilEnum.Usuario;

        // Act
        var entity = PerfilEntity.Criar(nome, email, senha, perfil);

        // Assert
        Assert.NotNull(entity);
        Assert.Equal(nome, entity.Nome);
        Assert.Equal(email, entity.Email);
        Assert.Equal(perfil, entity.Perfil);
        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.Equal(1, entity.Habilitado);
        Assert.True((DateTime.UtcNow - entity.DataCriacao).TotalSeconds < 5); // DataCriacao recente
        Assert.Null(entity.DataAlteracao);
        // Verifica se a senha foi hasheada corretamente
        Assert.True(BCrypt.Net.BCrypt.Verify(senha, entity.SenhaHash));
    }

    [Fact]
    public void Criar_DeveGerarNovoIdParaCadaInstancia() {
        // Arrange
        var nome = "Usuário Teste";
        var email = "usuario@teste.com";
        var senha = "senha123";
        var perfil = PerfilEnum.Usuario;

        // Act
        var entity1 = PerfilEntity.Criar(nome, email, senha, perfil);
        var entity2 = PerfilEntity.Criar(nome, email, senha, perfil);

        // Assert
        Assert.NotEqual(entity1.Id, entity2.Id);
    }
}
