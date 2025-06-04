using FCG.Domain.Repositories;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Enum;
using FCG.Domain.Shared.Results;
using FCG.Domain.Utils;
using Microsoft.Extensions.Logging;

namespace FCG.Domain.Services.Class;

/// <summary>
/// Serviço responsável pela autenticação de usuários e geração de tokens JWT.
/// </summary>
public class LoginServices(IPerfilRepository repository, IJwtServices jwtServices, ILoggerServices logger) : ILoginServices
{
    /// <summary>
    /// Realiza o login de um usuário, validando as credenciais e gerando um token JWT.
    /// </summary>
    /// <param name="request">Dados de login do usuário (e-mail e senha hash).</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo o token JWT e o ID do usuário (<see cref="LoginDto.LoginDtoResponse"/>)
    /// ou erro caso as credenciais estejam incorretas ou o usuário não seja encontrado.
    /// </returns>
    /// <remarks>
    /// Retorna erro caso o e-mail ou senha não sejam informados, usuário não seja encontrado ou senha inválida.
    /// </remarks>
    public async Task<Result<LoginDto.LoginDtoResponse>> Entrar(LoginDto.LoginDtoRequest request) {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.SenhaHash)) {
            await logger.LogError($"Email ou senha não informados.");
            return Result.Failure<LoginDto.LoginDtoResponse>("Email ou senha não informados.", "500");
        }

        var usuario = await repository.TrazUsuario(request.Email);
        if (usuario == null) {
            await logger.LogError($"Usuário não encontrado.");
            return Result.Failure<LoginDto.LoginDtoResponse>("Usuário não encontrado.", "500");
        }

        if (!ValidarSenha.Validar(request.SenhaHash, usuario.SenhaHash)) {
            await logger.LogError($"Senha inválida.");
            return Result.Failure<LoginDto.LoginDtoResponse>("Senha inválida.", "500");
        }

        var token = jwtServices.GenerateToken(usuario.Id, PerfilEnum.Administrador.ToString());

        var response = new LoginDto.LoginDtoResponse {
            Id = usuario.Id,
            Token = token
        };

        await logger.LogInformation("Finalizou EntrarNoSistema");
        return Result.Success(response);
    }
}