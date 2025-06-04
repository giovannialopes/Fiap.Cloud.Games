using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface IPerfilRepository : ICommit
{
    Task CriarPerfil(PerfilEntity usuario);
    Task<PerfilEntity> TrazUsuario(string email);
    Task AtualizaPerfil(PerfilEntity perfil);
}
