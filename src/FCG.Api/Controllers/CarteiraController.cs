using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static FCG.Domain.Shared.DTO.ErrorDto;
namespace FCG.Api.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de saldos da carteira dos usuários.
/// </summary>
[Route("api/v1")]
[ApiController]
public class CarteiraController(ICarteiraServices services, ILoggerServices logger) : ControllerBase
{
    /// <summary>
    /// Adiciona ou atualiza o saldo de um usuário na carteira.
    /// </summary>
    /// <param name="request">Dados do saldo a ser inserido ou atualizado (contendo o ID do usuário e o valor do saldo).</param>
    /// <returns>Retorna o saldo atualizado ou erro de validação.</returns>
    /// <response code="200">Saldo inserido/atualizado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpPost]
    [Route("adicionar/saldos")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> InserirSaldos(
        [FromBody] CarteiraDto.CarteiraDtoRequest request) {
        await logger.LogInformation("Iniciou InserirSaldos");
        var result = await services.InsereSaldos(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Consulta o saldo da carteira de um usuário específico.
    /// </summary>
    /// <param name="UsuarioId">ID do usuário para consulta do saldo.</param>
    /// <returns>Retorna o saldo encontrado ou erro de validação.</returns>
    /// <response code="200">Consulta realizada com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpGet]
    [Route("consulta/saldos")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConsultaSaldos(
        [FromHeader] Guid UsuarioId) {
        await logger.LogInformation("Iniciou ConsultaSaldos");
        var result = await services.ConsultaSaldos(UsuarioId);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Remove o saldo da carteira de um usuário.
    /// </summary>
    /// <param name="UsuarioId">ID do usuário para remoção do saldo.</param>
    /// <returns>Retorna confirmação da remoção ou erro de validação.</returns>
    /// <response code="200">Saldo removido com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpDelete]
    [Route("remover/saldos")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletarSaldos(
        [FromHeader] Guid UsuarioId) {
        await logger.LogInformation("Iniciou DeletarSaldos");
        var result = await services.RemoveSaldos(UsuarioId);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}