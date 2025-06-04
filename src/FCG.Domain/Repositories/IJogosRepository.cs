using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface IJogosRepository : ICommit
{
    Task AdicionarJogos(JogosEntity jogo);
    Task AtualizarJogos(JogosEntity jogo);
    Task<JogosEntity> ObterJogoPorNome(string jogo);
    Task<BibliotecaEntity> VerificaSeUsuarioPossuiJogo(string jogoId, string usuarioId);
    Task<List<JogosEntity>> ObterJogos();
    Task<List<JogosEntity>> ObterJogosPorUsuario(string Usuario);
}
