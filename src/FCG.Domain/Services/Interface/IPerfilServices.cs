using FCG.Domain.Entities;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;

namespace FCG.Domain.Services.Interface;

public interface IPerfilServices
{
    Task<Result<PerfilDto.PerfilResponse>> CriarPerfil(PerfilDto.PerfilRequest request);
    Task<Result<string>> AlterarPerfil(string email, string senha, PerfilDto.PerfilRequest request);
    Task<Result<string>> DeletarPerfil(string email, string senha);
}
