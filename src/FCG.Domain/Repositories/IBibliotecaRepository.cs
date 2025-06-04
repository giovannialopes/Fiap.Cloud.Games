using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface IBibliotecaRepository : ICommit
{
    Task AdicionaNaBiblioteca(BibliotecaEntity bibliotecaEntity);
}
