using FCG.Domain.Shared.Enum;

namespace FCG.Domain.Entities;

public class JogosEntity
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public decimal Preco { get; set; }
    public string Tipo { get; set; } = null!;
    public long Quantidade { get; set; } 
    public bool Ativo { get; set; } = true;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public static JogosEntity Criar(string nome, string descricao, decimal preco, string tipo, long quantidade ,bool ativo) {
        return new JogosEntity {
            Id = Guid.NewGuid(),
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            Tipo = tipo,
            Ativo = ativo,
            Quantidade = quantidade,
            DataCadastro = DateTime.UtcNow,
        };
    }
}
