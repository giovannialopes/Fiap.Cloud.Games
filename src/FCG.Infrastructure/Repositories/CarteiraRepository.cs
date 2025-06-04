using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public class CarteiraRepository : ICarteiraRepository
{
    private readonly DbFcg _dbFcg;
    public CarteiraRepository(DbFcg dbFcg) {
        _dbFcg = dbFcg;
    }


    public async Task Commit() =>
    await _dbFcg.
    SaveChangesAsync();

    public async Task AdicionaSaldo(CarteiraEntity carteira) => await _dbFcg.CARTEIRAS.AddAsync(carteira);

    public void AlteraSaldo(CarteiraEntity carteira) {
        _dbFcg.CARTEIRAS.Update(carteira);
        _dbFcg.SaveChanges();
    }

    public async Task<CarteiraEntity> ObtemSaldoPorId(Guid userId) =>
        await _dbFcg.CARTEIRAS.AsNoTracking().FirstOrDefaultAsync(x => x.UsuarioId == userId);
}
