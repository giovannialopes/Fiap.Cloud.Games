using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace FCG.Domain.Services.Class;

/// <summary>
/// Serviço responsável pelas operações de saldo da carteira dos usuários.
/// </summary>
public class CarteiraServices(ICarteiraRepository carteiraRepository, ILoggerServices logger) : ICarteiraServices
{
    /// <summary>
    /// Consulta o saldo da carteira de um usuário específico.
    /// </summary>
    /// <param name="UsuarioId">Identificador do usuário.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo o saldo da carteira (<see cref="CarteiraDto.CarteiraDtoResponse"/>)
    /// ou erro caso o saldo não seja encontrado.
    /// </returns>
    public async Task<Result<CarteiraDto.CarteiraDtoResponse>> ConsultaSaldos(Guid UsuarioId) {
        var carteira = await carteiraRepository.ObtemSaldoPorId(UsuarioId);

        if (carteira == null) {
            await logger.LogError($"Saldo não encontrado.");
            return Result.Failure<CarteiraDto.CarteiraDtoResponse>("Saldo não encontrado.", "500");
        }

        await logger.LogInformation("Finalizou ConsultaSaldos");
        return Result.Success(new CarteiraDto.CarteiraDtoResponse { Saldo = carteira.Saldo });
    }

    /// <summary>
    /// Insere ou atualiza o saldo da carteira de um usuário.
    /// </summary>
    /// <param name="request">Dados para inserção ou atualização do saldo (ID do usuário e valor do saldo).</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo o saldo atualizado (<see cref="CarteiraDto.CarteiraDtoResponse"/>).
    /// </returns>
    public async Task<Result<CarteiraDto.CarteiraDtoResponse>> InsereSaldos(CarteiraDto.CarteiraDtoRequest request) {
        var carteira = await carteiraRepository.ObtemSaldoPorId(request.UsuarioId);
        if (carteira != null) {
            var novaCarteira = CarteiraEntity.Atualizar(
                carteira.Id,
                carteira.UsuarioId,
                carteira.Saldo + request.Saldo);

            carteiraRepository.AlteraSaldo(novaCarteira);

            return Result.Success(new CarteiraDto.CarteiraDtoResponse {
                Saldo = novaCarteira.Saldo
            });
        }

        var saldo = CarteiraEntity.Criar(request.UsuarioId, request.Saldo);

        await carteiraRepository.AdicionaSaldo(saldo);

        await carteiraRepository.Commit();

        await logger.LogInformation("Finalizou InserirSaldos");

        return Result.Success(new CarteiraDto.CarteiraDtoResponse {
            Saldo = saldo.Saldo
        });
    }

    /// <summary>
    /// Remove o saldo da carteira de um usuário, zerando o valor.
    /// </summary>
    /// <param name="UsuarioId">Identificador do usuário.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo o saldo zerado (<see cref="CarteiraDto.CarteiraDtoResponse"/>).
    /// </returns>
    public async Task<Result<CarteiraDto.CarteiraDtoResponse>> RemoveSaldos(Guid UsuarioId) {
        var carteira = await carteiraRepository.ObtemSaldoPorId(UsuarioId);

        carteira.Saldo = 0;

        carteiraRepository.AlteraSaldo(carteira);

        await logger.LogInformation("Finalizou DeletarSaldos");
        return Result.Success(new CarteiraDto.CarteiraDtoResponse {
            Saldo = carteira.Saldo
        });
    }
}