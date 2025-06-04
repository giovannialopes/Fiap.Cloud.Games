using FCG.Domain.Repositories;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;

namespace FCG.Domain.Services.Interface;

public interface IBibliotecaServices
{
    Task<Result<List<JogosDto.JogosDtoResponse>>> ConsultarBibliotecaPorUsuario(string Usuario);
}
