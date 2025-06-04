using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;

namespace FCG.Domain.Services.Interface;

public interface ILoginServices
{
    Task<Result<LoginDto.LoginDtoResponse>> Entrar(LoginDto.LoginDtoRequest request);
}
