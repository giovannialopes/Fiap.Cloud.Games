using System.Text.Json;
using FCG.Domain.Entities;
using FCG.Domain.Shared.Enum;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Tests.Infrastructure.Data;

public class DbFcgTests
{
    private DbContextOptions<DbFcg> CreateOptions() {
        return new DbContextOptionsBuilder<DbFcg>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Deve_Criar_DbSets() {
        using var context = new DbFcg(CreateOptions());
        Assert.NotNull(context.USERS);
        Assert.NotNull(context.CARTEIRAS);
        Assert.NotNull(context.JOGOS);
        Assert.NotNull(context.BIBLIOTECA_DE_JOGOS);
        Assert.NotNull(context.PROMOCOES);
    }

    [Fact]
    public async Task Deve_Permitir_Persistir_PerfilEntity() {
        using var context = new DbFcg(CreateOptions());
        var perfil = PerfilEntity.Criar("Usuário", "email@teste.com", "senha", PerfilEnum.Usuario);

        context.USERS.Add(perfil);
        await context.SaveChangesAsync();

        var recuperado = await context.USERS.FirstOrDefaultAsync(x => x.Id == perfil.Id);
        Assert.NotNull(recuperado);
        Assert.Equal(perfil.Email, recuperado.Email);
    }

    [Fact]
    public async Task Deve_Permitir_Persistir_CarteiraEntity() {
        using var context = new DbFcg(CreateOptions());
        var carteira = CarteiraEntity.Criar(Guid.NewGuid(), 123.45m);

        context.CARTEIRAS.Add(carteira);
        await context.SaveChangesAsync();

        var recuperada = await context.CARTEIRAS.FirstOrDefaultAsync(x => x.Id == carteira.Id);
        Assert.NotNull(recuperada);
        Assert.Equal(carteira.Saldo, recuperada.Saldo);
    }

    [Fact]
    public async Task Deve_Permitir_Persistir_JogosEntity() {
        using var context = new DbFcg(CreateOptions());
        var jogo = JogosEntity.Criar("Jogo", "Desc", 99.99m, "Ação", 10, true);

        context.JOGOS.Add(jogo);
        await context.SaveChangesAsync();

        var recuperado = await context.JOGOS.FirstOrDefaultAsync(x => x.Id == jogo.Id);
        Assert.NotNull(recuperado);
        Assert.Equal(jogo.Nome, recuperado.Nome);
    }

    [Fact]
    public async Task Deve_Permitir_Persistir_BibliotecaEntity() {
        using var context = new DbFcg(CreateOptions());
        var biblioteca = BibliotecaEntity.Criar(Guid.NewGuid(), Guid.NewGuid());

        context.BIBLIOTECA_DE_JOGOS.Add(biblioteca);
        await context.SaveChangesAsync();

        var recuperada = await context.BIBLIOTECA_DE_JOGOS.FirstOrDefaultAsync(x => x.Id == biblioteca.Id);
        Assert.NotNull(recuperada);
        Assert.Equal(biblioteca.JogoId, recuperada.JogoId);
    }

    [Fact]
    public async Task Deve_Permitir_Persistir_PromocoesEntity_Com_IdJogos() {
        using var context = new DbFcg(CreateOptions());
        var idJogos = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var promocao = PromocoesEntity.Criar("Promoção", 10m, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), idJogos);

        context.PROMOCOES.Add(promocao);
        await context.SaveChangesAsync();

        var recuperada = await context.PROMOCOES.FirstOrDefaultAsync(x => x.Id == promocao.Id);
        Assert.NotNull(recuperada);
        Assert.Equal(promocao.Nome, recuperada.Nome);
        Assert.Equal(idJogos, recuperada.IdJogos);
    }

    [Fact]
    public void Deve_Configurar_Precisao_Decimal() {
        using var context = new DbFcg(CreateOptions());
        var model = context.Model;

        var carteiraSaldo = model.FindEntityType(typeof(CarteiraEntity))?.FindProperty(nameof(CarteiraEntity.Saldo));
        var jogosPreco = model.FindEntityType(typeof(JogosEntity))?.FindProperty(nameof(JogosEntity.Preco));
        var promocoesValor = model.FindEntityType(typeof(PromocoesEntity))?.FindProperty(nameof(PromocoesEntity.Valor));

        Assert.Equal(18, carteiraSaldo?.GetPrecision());
        Assert.Equal(2, carteiraSaldo?.GetScale());
        Assert.Equal(18, jogosPreco?.GetPrecision());
        Assert.Equal(2, jogosPreco?.GetScale());
        Assert.Equal(18, promocoesValor?.GetPrecision());
        Assert.Equal(2, promocoesValor?.GetScale());
    }

    [Fact]
    public void Deve_Configurar_Conversao_IdJogos() {
        using var context = new DbFcg(CreateOptions());
        var model = context.Model;
        var prop = model.FindEntityType(typeof(PromocoesEntity))?.FindProperty(nameof(PromocoesEntity.IdJogos));
        Assert.NotNull(prop);
        Assert.NotNull(prop.GetValueConverter());
    }
}
