using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Enum;
using FCG.Domain.Shared.Results;
using FCG.Domain.Utils;
using Microsoft.Extensions.Logging;

namespace FCG.Domain.Services.Class;

/// <summary>
/// Serviço responsável pelo gerenciamento de perfis de usuário, incluindo criação, alteração e exclusão.
/// </summary>
public class PerfilServices(IPerfilRepository repository, IJwtServices jwtServices, ILoggerServices logger) : IPerfilServices
{
    /// <summary>
    /// Cria um novo perfil de usuário.
    /// </summary>
    /// <param name="request">Dados do perfil a ser criado.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo o ID do perfil criado e o token JWT (<see cref="PerfilDto.PerfilResponse"/>),
    /// ou erro caso ocorra alguma falha na criação.
    /// </returns>
    public async Task<Result<PerfilDto.PerfilResponse>> CriarPerfil(PerfilDto.PerfilRequest request) {

        var perfil = PerfilEntity.Criar(
            request.Nome,
            request.Email,
            request.SenhaHash,
            request.PerfilEnum
        );

        try {
            await repository.CriarPerfil(perfil);
            await repository.Commit();

            var token = jwtServices.GenerateToken(perfil.Id, request.PerfilEnum == PerfilEnum.Usuario ? PerfilEnum.Usuario.ToString() : PerfilEnum.Administrador.ToString());

            await logger.LogInformation("Finalizou CriarPerfil");
            return Result.Success(new PerfilDto.PerfilResponse {
                Id = perfil.Id,
                Token = token
            });
        }
        catch (Exception ex) {
            return Result.Failure<PerfilDto.PerfilResponse>($"Erro ao criar perfil usuário", "500");
        }
    }

    /// <summary>
    /// Altera os dados de um perfil de usuário existente.
    /// </summary>
    /// <param name="email">E-mail do usuário a ser alterado.</param>
    /// <param name="senha">Senha do usuário para validação.</param>
    /// <param name="request">Novos dados do perfil.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo mensagem de sucesso ou erro caso as credenciais estejam incorretas ou o usuário não seja encontrado.
    /// </returns>
    public async Task<Result<string>> AlterarPerfil(string email, string senha, PerfilDto.PerfilRequest request) {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha)) {
            await logger.LogError($"Email ou senha não informados.");
            return Result.Failure<string>("Email ou senha não informados.", "500");
        }

        var usuario = await repository.TrazUsuario(email);
        if (usuario == null) {
            await logger.LogError($"Usuário não encontrado para alteração.");
            return Result.Failure<string>("Usuário não encontrado para alteração.", "500");
        }

        if (!ValidarSenha.Validar(senha, usuario.SenhaHash)) {
            await logger.LogError($"Senha inválida para alteração.");
            return Result.Failure<string>("Senha inválida para alteração.", "500");
        }

        usuario.Nome = request.Nome;
        usuario.Email = request.Email;
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.SenhaHash);
        usuario.Perfil = request.PerfilEnum;
        usuario.DataAlteracao = DateTime.UtcNow;

        await repository.AtualizaPerfil(usuario);
        await logger.LogInformation("Finalizou AlterarPerfil");
        return Result.Success("Perfil atualizado com sucesso.");
    }

    /// <summary>
    /// Desativa (deleta logicamente) um perfil de usuário.
    /// </summary>
    /// <param name="email">E-mail do usuário a ser desativado.</param>
    /// <param name="senha">Senha do usuário para validação.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo mensagem de sucesso ou erro caso as credenciais estejam incorretas ou o usuário não seja encontrado.
    /// </returns>
    public async Task<Result<string>> DeletarPerfil(string email, string senha) {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha)) {
            await logger.LogError($"Email ou senha não informados.");
            return Result.Failure<string>("Email ou senha não informados.", "500");
        }

        var usuario = await repository.TrazUsuario(email);
        if (usuario == null) {
            await logger.LogError($"Usuário não encontrado para alteração.");
            return Result.Failure<string>("Usuário não encontrado para alteração.", "500");
        }

        if (!ValidarSenha.Validar(senha, usuario.SenhaHash)) {
            await logger.LogError($"Senha inválida para alteração.");
            return Result.Failure<string>("Senha inválida para alteração.", "500");
        }

        usuario.Habilitado = 0;
        usuario.DataAlteracao = DateTime.UtcNow;

        await repository.AtualizaPerfil(usuario);
        await logger.LogInformation("Finalizou DeletarPerfil");
        return Result.Success("Perfil desativado com sucesso.");
    }
}