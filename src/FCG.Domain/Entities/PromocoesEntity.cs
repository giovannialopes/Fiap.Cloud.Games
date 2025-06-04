namespace FCG.Domain.Entities;

public class PromocoesEntity
{
    public Guid Id { get; set; } 
    public string Nome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public List<Guid> IdJogos { get; set; } = new();


    public static PromocoesEntity Criar(string nome, decimal valor, DateTime datainicio, DateTime datafim, List<Guid> idjogos) {
        return new PromocoesEntity {
            Id = Guid.NewGuid(),
            Nome = nome,
            Valor = valor,
            DataInicio = datainicio,
            DataFim = datafim,
            IdJogos = idjogos
        };
    }
}
