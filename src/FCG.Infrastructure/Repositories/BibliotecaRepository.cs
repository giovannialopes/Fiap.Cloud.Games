using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.Data;

namespace FCG.Infrastructure.Repositories;

public class BibliotecaRepository : IBibliotecaRepository
{
    private readonly DbFcg _dbFcg;

    public BibliotecaRepository(DbFcg dbFcg) {
        _dbFcg = dbFcg;
    }

    public async Task AdicionaNaBiblioteca(BibliotecaEntity bibliotecaEntity) {

        await _dbFcg.BIBLIOTECA_DE_JOGOS.AddAsync(bibliotecaEntity);
        await Commit();
    }

    public Task Commit() => _dbFcg.SaveChangesAsync();
}
