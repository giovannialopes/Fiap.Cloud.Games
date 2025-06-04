using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Repositories;

public class JogosRepository : IJogosRepository
{
    private readonly DbFcg _dbFcg;

    public JogosRepository(DbFcg dbFcg) {
        _dbFcg = dbFcg;
    }

    public async Task Commit() =>
        await _dbFcg.
        SaveChangesAsync();

    public async Task AdicionarJogos(JogosEntity jogo) {
        await _dbFcg.JOGOS.AddAsync(jogo);
        await Commit();
    }


    public async Task AtualizarJogos(JogosEntity jogo) {
        _dbFcg.JOGOS.Update(jogo);
        await Commit();
    }

    public async Task<JogosEntity> ObterJogoPorNome(string jogo) =>
        await _dbFcg.JOGOS.AsNoTracking().FirstOrDefaultAsync(x => x.Nome.Equals(jogo) && x.Ativo == true);

    public async Task<List<JogosEntity>> ObterJogos() =>
        await _dbFcg.JOGOS.AsNoTracking().ToListAsync();

    public async Task<List<JogosEntity>> ObterJogosPorUsuario(string Usuario) {
        var jogos = await (
        from b in _dbFcg.BIBLIOTECA_DE_JOGOS
        join j in _dbFcg.JOGOS on b.JogoId equals j.Id
        where b.UsuarioId == Guid.Parse(Usuario)
        select j).ToListAsync();


        return jogos;
    }

    public async Task<BibliotecaEntity> VerificaSeUsuarioPossuiJogo(string jogoId, string usuarioId) =>
        await _dbFcg.BIBLIOTECA_DE_JOGOS
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.JogoId.Equals(Guid.Parse(jogoId)) &&
                x.UsuarioId.Equals(Guid.Parse(usuarioId))
            );


}
