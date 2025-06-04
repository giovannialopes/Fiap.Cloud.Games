using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static FCG.Domain.Shared.DTO.ErrorDto;

namespace FCG.Api.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de promoções no sistema.
/// </summary>
[Route("api/v1")]
[ApiController]
public class PromocaoController(IPromocoesServices services, ILoggerServices logger) : ControllerBase
{
    /// <summary>
    /// Adiciona uma nova promoção ao sistema.
    /// </summary>
    /// <param name="request">Dados da promoção a ser cadastrada (nome, valor, datas e jogos associados).</param>
    /// <returns>Retorna os dados da promoção cadastrada ou erro de validação.</returns>
    /// <response code="200">Promoção cadastrada com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpPost]
    [Route("adicionar/promocoes")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> InserirPromocoes(
        [FromBody] PromocoesDto.PromocoesDtoRequest request) {
        await logger.LogInformation("Iniciou InserirPromocoes");
        var result = await services.InserePromocoes(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Consulta todas as promoções ativas no sistema.
    /// </summary>
    /// <returns>Retorna a lista de promoções ativas ou erro de validação.</returns>
    /// <response code="200">Consulta realizada com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpGet]
    [Route("consulta/promocoes")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConsultarPromocoes() {
        await logger.LogInformation("Iniciou ConsultarPromocoes");
        var result = await services.ConsultaPromocoesAtivas();
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}