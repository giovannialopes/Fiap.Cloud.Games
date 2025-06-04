using FCG.Domain.Shared.DTO;
using System.ComponentModel.DataAnnotations;

namespace FCG.Tests.Domain.Shared.DTO;

public class JogosDtoTests
{
    [Fact]
    public void JogosDtoRequest_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var nome = "Jogo Teste";
        var preco = 99.99m;
        var descricao = "Descrição do jogo";
        var quantidade = 10L;
        var tipo = "Aventura";

        // Act
        var dto = new JogosDto.JogosDtoRequest {
            Nome = nome,
            Preco = preco,
            Descricao = descricao,
            Quantidade = quantidade,
            Tipo = tipo
        };

        // Assert
        Assert.Equal(nome, dto.Nome);
        Assert.Equal(preco, dto.Preco);
        Assert.Equal(descricao, dto.Descricao);
        Assert.Equal(quantidade, dto.Quantidade);
        Assert.Equal(tipo, dto.Tipo);
    }

    [Fact]
    public void JogosDtoResponse_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var nome = "Jogo Teste";
        var preco = 49.99m;
        var descricao = "Outro jogo";
        var quantidade = 5L;
        var tipo = "RPG";

        // Act
        var dto = new JogosDto.JogosDtoResponse {
            Nome = nome,
            Preco = preco,
            Descricao = descricao,
            Quantidade = quantidade,
            Tipo = tipo
        };

        // Assert
        Assert.Equal(nome, dto.Nome);
        Assert.Equal(preco, dto.Preco);
        Assert.Equal(descricao, dto.Descricao);
        Assert.Equal(quantidade, dto.Quantidade);
        Assert.Equal(tipo, dto.Tipo);
    }

    [Fact]
    public void JogosDtoComprarJogos_DevePermitirSetarEObterNome() {
        // Arrange
        var nome = "Jogo Para Comprar";

        // Act
        var dto = new JogosDto.JogosDtoComprarJogos {
            Nome = nome
        };

        // Assert
        Assert.Equal(nome, dto.Nome);
    }
}
