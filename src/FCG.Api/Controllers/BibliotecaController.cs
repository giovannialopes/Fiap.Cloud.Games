using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static FCG.Domain.Shared.DTO.ErrorDto;

namespace FCG.Api.Controllers;

/// <summary>
/// Controller responsável pela consulta da biblioteca de jogos do usuário.
/// </summary>
[Route("api/v1")]
[ApiController]
public class BibliotecaController(IBibliotecaServices services, ILoggerServices logger) : ControllerBase
{
    /// <summary>
    /// Consulta a biblioteca de jogos do usuário autenticado.
    /// </summary>
    /// <remarks>
    /// Retorna a lista de jogos pertencentes ao usuário autenticado, identificando-o pelo token JWT.
    /// </remarks>
    /// <returns>Lista de jogos da biblioteca do usuário ou erro de validação.</returns>
    /// <response code="200">Consulta realizada com sucesso. Retorna a lista de jogos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpGet]
    [Route("consultar/usuario/biblioteca")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConsultarBiblioteca() {
        await logger.LogInformation("Iniciou ConsultarBiblioteca");
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await services.ConsultarBibliotecaPorUsuario(userIdString!);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}