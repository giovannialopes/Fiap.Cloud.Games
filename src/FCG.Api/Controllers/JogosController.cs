using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static FCG.Domain.Shared.DTO.ErrorDto;

namespace FCG.Api.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de jogos no sistema.
/// </summary>
[Route("api/v1")]
[ApiController]
public class JogosController([FromServices] IJogosServices services, ILoggerServices logger) : ControllerBase
{
    /// <summary>
    /// Cadastra um novo jogo no sistema.
    /// </summary>
    /// <param name="request">Dados do jogo a ser cadastrado.</param>
    /// <returns>Retorna os dados do jogo cadastrado ou erro de validação.</returns>
    /// <response code="200">Jogo cadastrado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpPost]
    [Route("cadastrar/jogos")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CadastrarJogos(
        [FromBody] JogosDto.JogosDtoRequest request) {
        await logger.LogInformation("Iniciou CadastrarJogos");
        var result = await services.CadastrarJogos(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Realiza a compra de um ou mais jogos pelo usuário autenticado.
    /// </summary>
    /// <param name="request">Dados do(s) jogo(s) a ser(em) comprado(s).</param>
    /// <returns>Retorna os dados da compra ou erro de validação.</returns>
    /// <response code="200">Compra realizada com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpPost]
    [Route("comprar/jogos")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ComprarJogos(
        [FromBody] JogosDto.JogosDtoComprarJogos request) {
        await logger.LogInformation("Iniciou ComprarJogos");
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await services.ComprarJogos(request, userIdString!);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Altera os dados de um jogo existente.
    /// </summary>
    /// <param name="request">Novos dados do jogo.</param>
    /// <param name="Nome">Nome do jogo a ser alterado.</param>
    /// <returns>Retorna os dados do jogo alterado ou erro de validação.</returns>
    /// <response code="200">Jogo alterado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpPut]
    [Route("alterar/jogos")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AlterarJogos(
        [FromBody] JogosDto.JogosDtoRequest request,
        [FromQuery] string Nome) {
        await logger.LogInformation("Iniciou AlterarJogos");
        var result = await services.AlterarJogos(Nome, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Consulta os dados de um jogo específico pelo nome.
    /// </summary>
    /// <param name="NomeDoJogo">Nome do jogo a ser consultado.</param>
    /// <returns>Retorna os dados do jogo ou erro de validação.</returns>
    /// <response code="200">Consulta realizada com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpGet]
    [Route("consultar/unico/jogo")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConsultarUnicoJogo(
        [FromHeader] string NomeDoJogo) {
        await logger.LogInformation("Iniciou ConsultarUnicoJogo");
        var result = await services.ConsultarUnicoJogo(NomeDoJogo);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Consulta todos os jogos cadastrados no sistema.
    /// </summary>
    /// <returns>Retorna a lista de jogos ou erro de validação.</returns>
    /// <response code="200">Consulta realizada com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpGet]
    [Route("consultar/jogos")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConsultarJogos() {
        await logger.LogInformation("Iniciou ConsultarJogos");
        var result = await services.ConsultarJogos();
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Deleta um jogo do sistema pelo nome.
    /// </summary>
    /// <param name="NomeDoJogo">Nome do jogo a ser deletado.</param>
    /// <returns>Retorna confirmação de exclusão ou erro de validação.</returns>
    /// <response code="200">Jogo deletado com sucesso.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    [HttpDelete]
    [Route("deletar/jogos")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletarJogos(
        [FromHeader] string NomeDoJogo) {
        await logger.LogInformation("Iniciou DeletarJogos");
        var result = await services.DeletarJogos(NomeDoJogo);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}