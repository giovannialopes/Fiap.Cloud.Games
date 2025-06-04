using FCG.Domain.Shared.DTO;

namespace FCG.Tests.Domain.Shared.DTO;

public class PromocoesDtoTests
{
    [Fact]
    public void PromocoesDtoRequest_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var nome = "Promoção de Verão";
        var valor = 50.5m;
        var dataInicio = new DateTime(2025, 1, 1);
        var dataFim = new DateTime(2025, 1, 10);
        var idJogos = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        // Act
        var dto = new PromocoesDto.PromocoesDtoRequest {
            Nome = nome,
            Valor = valor,
            DataInicio = dataInicio,
            DataFim = dataFim,
            IdJogos = idJogos
        };

        // Assert
        Assert.Equal(nome, dto.Nome);
        Assert.Equal(valor, dto.Valor);
        Assert.Equal(dataInicio, dto.DataInicio);
        Assert.Equal(dataFim, dto.DataFim);
        Assert.Equal(idJogos, dto.IdJogos);
    }

    [Fact]
    public void PromocoesDtoRequest_ValoresPadraoDevemSerCorretos() {
        // Act
        var dto = new PromocoesDto.PromocoesDtoRequest();

        // Assert
        Assert.Equal(string.Empty, dto.Nome);
        Assert.Equal(0m, dto.Valor);
        Assert.Equal(default, dto.DataInicio);
        Assert.Equal(default, dto.DataFim);
        Assert.NotNull(dto.IdJogos);
        Assert.Empty(dto.IdJogos);
    }

    [Fact]
    public void PromocoesDtoResponse_DevePermitirSetarEObterPropriedades() {
        // Arrange
        var nome = "Promoção de Inverno";
        var valor = 30m;
        var dataInicio = new DateTime(2025, 6, 1);
        var dataFim = new DateTime(2025, 6, 15);
        var idJogos = new List<Guid> { Guid.NewGuid() };

        // Act
        var dto = new PromocoesDto.PromocoesDtoResponse {
            Nome = nome,
            Valor = valor,
            DataInicio = dataInicio,
            DataFim = dataFim,
            IdJogos = idJogos
        };

        // Assert
        Assert.Equal(nome, dto.Nome);
        Assert.Equal(valor, dto.Valor);
        Assert.Equal(dataInicio, dto.DataInicio);
        Assert.Equal(dataFim, dto.DataFim);
        Assert.Equal(idJogos, dto.IdJogos);
    }

    [Fact]
    public void PromocoesDtoResponse_ValoresPadraoDevemSerCorretos() {
        // Act
        var dto = new PromocoesDto.PromocoesDtoResponse();

        // Assert
        Assert.Equal(string.Empty, dto.Nome);
        Assert.Equal(0m, dto.Valor);
        Assert.Equal(default, dto.DataInicio);
        Assert.Equal(default, dto.DataFim);
        Assert.NotNull(dto.IdJogos);
        Assert.Empty(dto.IdJogos);
    }
}
