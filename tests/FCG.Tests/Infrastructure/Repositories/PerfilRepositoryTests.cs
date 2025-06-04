using System;
using System.Threading.Tasks;
using FCG.Domain.Entities;
using FCG.Domain.Shared.Enum;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FCG.Tests.Infrastructure.Repositories;

public class PerfilRepositoryTests
{
    private DbFcg CreateContext() {
        var options = new DbContextOptionsBuilder<DbFcg>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new DbFcg(options);
    }

    [Fact]
    public async Task CriarPerfil_DevePersistirEntidade() {
        // Arrange
        using var context = CreateContext();
        var repo = new PerfilRepository(context);
        var perfil = PerfilEntity.Criar("Usuário Teste", "usuario@teste.com", "senha123", PerfilEnum.Usuario);

        // Act
        await repo.CriarPerfil(perfil);
        await repo.Commit();

        // Assert
        var encontrado = await context.USERS.FirstOrDefaultAsync(x => x.Id == perfil.Id);
        Assert.NotNull(encontrado);
        Assert.Equal("usuario@teste.com", encontrado.Email);
        Assert.Equal(PerfilEnum.Usuario, encontrado.Perfil);
    }

    [Fact]
    public async Task TrazUsuario_DeveRetornarUsuarioPorEmail() {
        // Arrange
        using var context = CreateContext();
        var repo = new PerfilRepository(context);
        var perfil = PerfilEntity.Criar("Usuário Teste", "usuario@teste.com", "senha123", PerfilEnum.Usuario);
        await context.USERS.AddAsync(perfil);
        await context.SaveChangesAsync();

        // Act
        var encontrado = await repo.TrazUsuario("usuario@teste.com");

        // Assert
        Assert.NotNull(encontrado);
        Assert.Equal(perfil.Id, encontrado.Id);
        Assert.Equal("usuario@teste.com", encontrado.Email);
    }

    [Fact]
    public async Task TrazUsuario_DeveRetornarNull_QuandoEmailNaoExiste() {
        // Arrange
        using var context = CreateContext();
        var repo = new PerfilRepository(context);

        // Act
        var encontrado = await repo.TrazUsuario("naoexiste@teste.com");

        // Assert
        Assert.Null(encontrado);
    }

    [Fact]
    public async Task Commit_DeveSalvarAlteracoes() {
        // Arrange
        using var context = CreateContext();
        var repo = new PerfilRepository(context);
        var perfil = PerfilEntity.Criar("Usuário Commit", "commit@teste.com", "senha", PerfilEnum.Administrador);

        // Act
        await context.USERS.AddAsync(perfil);
        await repo.Commit();

        // Assert
        var encontrado = await context.USERS.FirstOrDefaultAsync(x => x.Id == perfil.Id);
        Assert.NotNull(encontrado);
    }

}
