using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public class PerfilRepository : IPerfilRepository
{
    private readonly DbFcg _dbFcg;

    public PerfilRepository(DbFcg dbFcg) {
        _dbFcg = dbFcg;
    }

    public async Task AtualizaPerfil(PerfilEntity perfil) {
        _dbFcg.USERS.Update(perfil);
        await Commit();
    }


    public async Task Commit() =>
        await _dbFcg.SaveChangesAsync();

    public async Task CriarPerfil(PerfilEntity usuario) =>
        await _dbFcg.USERS.
        AddAsync(usuario);

    public async Task<PerfilEntity> TrazUsuario(string email) =>
        await _dbFcg.USERS.
        AsNoTracking().
        FirstOrDefaultAsync(x => x.Email == email && x.Habilitado == 1);


}
