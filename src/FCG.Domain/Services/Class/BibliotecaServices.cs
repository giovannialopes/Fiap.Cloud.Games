using FCG.Domain.Repositories;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace FCG.Domain.Services.Class;

/// <summary>
/// Serviço responsável pelas operações de consulta da biblioteca de jogos de um usuário.
/// </summary>
public class BibliotecaServices(IJogosRepository jogosRepository, ILoggerServices logger) : IBibliotecaServices
{
    /// <summary>
    /// Consulta a biblioteca de jogos de um usuário específico.
    /// </summary>
    /// <param name="Usuario">Identificador do usuário (string, geralmente o ID ou e-mail).</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo a lista de jogos (<see cref="JogosDto.JogosDtoResponse"/>) pertencentes ao usuário,
    /// ou um erro caso o usuário não possua jogos na biblioteca.
    /// </returns>
    /// <remarks>
    /// Retorna erro 404 caso o usuário não possua jogos na biblioteca.
    /// </remarks>
    public async Task<Result<List<JogosDto.JogosDtoResponse>>> ConsultarBibliotecaPorUsuario(string Usuario) {
        var jogos = await jogosRepository.ObterJogosPorUsuario(Usuario);

        if (!jogos.Any()) {
            await logger.LogError($"Usuário {Usuario} não possui jogos na biblioteca.");
            return Result.Failure<List<JogosDto.JogosDtoResponse>>("Usuário não possui jogos na biblioteca.", "404");
        }

        var response = jogos.Select(x => new JogosDto.JogosDtoResponse {
            Nome = x.Nome,
            Descricao = x.Descricao,
            Preco = x.Preco,
            Tipo = x.Tipo,
            Quantidade = x.Quantidade
        }).ToList();

        await logger.LogInformation("Finalizou ConsultarBiblioteca");
        return Result.Success(response);
    }
}