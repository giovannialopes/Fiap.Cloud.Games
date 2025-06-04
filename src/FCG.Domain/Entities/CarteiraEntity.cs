namespace FCG.Domain.Entities;

public class CarteiraEntity
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public decimal Saldo { get; set; }


    public static CarteiraEntity Criar(Guid userId, decimal saldo) {
        return new CarteiraEntity {
            Id = Guid.NewGuid(),
            Saldo = saldo,
            UsuarioId = userId

        };
    }

    public static CarteiraEntity Atualizar(Guid id, Guid userId, decimal saldo) {
        return new CarteiraEntity {
            Id = id,
            Saldo = saldo,
            UsuarioId = userId

        };
    }
}


