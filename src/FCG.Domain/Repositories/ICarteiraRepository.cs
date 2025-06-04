using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface ICarteiraRepository : ICommit
{
    Task<CarteiraEntity> ObtemSaldoPorId(Guid usuarioId);
    Task AdicionaSaldo(CarteiraEntity carteira);
    void AlteraSaldo(CarteiraEntity carteira);

}
