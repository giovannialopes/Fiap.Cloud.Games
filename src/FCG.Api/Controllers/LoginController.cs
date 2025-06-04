using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using static FCG.Domain.Shared.DTO.ErrorDto;

namespace FCG.Api.Controllers;

/// <summary>
/// Controller responsável pelo login de usuários no sistema.
/// </summary>
[Route("api/v1")]
[ApiController]
public class LoginController([FromServices] ILoginServices services, ILoggerServices logger) : ControllerBase
{
    /// <summary>
    /// Realiza o login de um usuário no sistema.
    /// </summary>
    /// <param name="request">Dados de login do usuário (e-mail e senha hash).</param>
    /// <returns>Retorna um token de autenticação em caso de sucesso ou erro de autenticação.</returns>
    /// <response code="200">Login realizado com sucesso. Retorna o token de autenticação.</response>
    /// <response code="401">Não autorizado. Credenciais inválidas.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpPost]
    [Route("entrar/usuario")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> EntrarNoSistema(
        [FromBody] LoginDto.LoginDtoRequest request) {
        await logger.LogInformation("Iniciou EntrarNoSistema");
        var result = await services.Entrar(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}