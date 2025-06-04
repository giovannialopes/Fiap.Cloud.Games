using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;

namespace FCG.Domain.Services.Interface;

public interface ICarteiraServices
{
    Task<Result<CarteiraDto.CarteiraDtoResponse>> InsereSaldos(CarteiraDto.CarteiraDtoRequest request);
    Task<Result<CarteiraDto.CarteiraDtoResponse>> ConsultaSaldos(Guid UsuarioId);
    Task<Result<CarteiraDto.CarteiraDtoResponse>> RemoveSaldos(Guid UsuarioId);
}
