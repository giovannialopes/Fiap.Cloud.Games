namespace FCG.Domain.Shared.DTO;

public class PromocoesDto
{
    public class PromocoesDtoRequest
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<Guid> IdJogos { get; set; } = new();
    }

    public class PromocoesDtoResponse
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<Guid> IdJogos { get; set; } = new();
    }
}
