using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FCG.Domain.Shared.DTO.ErrorDto;

namespace FCG.Api.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de perfis de usuário.
/// </summary>
[Route("api/v1")]
[ApiController]
public class PerfilController([FromServices] IPerfilServices services, ILoggerServices logger) : ControllerBase
{
    /// <summary>
    /// Cria um novo perfil de usuário.
    /// </summary>
    /// <param name="request">Dados do perfil a ser criado.</param>
    /// <returns>Retorna o token do perfil criado ou erro de validação.</returns>
    /// <response code="200">Perfil criado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpPost]
    [Route("criar/perfil")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CriarPerfil(
        [FromBody] PerfilDto.PerfilRequest request) {
        await logger.LogInformation("Iniciou CriarPerfil");
        var result = await services.CriarPerfil(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Altera um perfil de usuário existente.
    /// </summary>
    /// <param name="request">Novos dados do perfil.</param>
    /// <param name="email">E-mail do perfil a ser alterado.</param>
    /// <param name="senha">Senha do perfil a ser alterado.</param>
    /// <returns>Retorna mensagem de sucesso ou erro de validação.</returns>
    /// <response code="200">Perfil alterado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpPut]
    [Route("alterar/perfil")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AlterarPerfil(
        [FromBody] PerfilDto.PerfilRequest request,
        [FromQuery] string email,
        [FromQuery] string senha) {
        await logger.LogInformation("Iniciou AlterarPerfil");
        var result = await services.AlterarPerfil(email, senha, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Deleta um perfil de usuário.
    /// </summary>
    /// <param name="email">E-mail do perfil a ser deletado.</param>
    /// <param name="senha">Senha do perfil a ser deletado.</param>
    /// <returns>Retorna mensagem de sucesso ou erro de validação.</returns>
    /// <response code="200">Perfil deletado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpDelete]
    [Route("deletar/perfil")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletarPerfil(
        [FromQuery] string email,
        [FromQuery] string senha) {
        await logger.LogInformation("Iniciou DeletarPerfil");
        var result = await services.DeletarPerfil(email, senha);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}