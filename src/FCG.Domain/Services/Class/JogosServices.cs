using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace FCG.Domain.Services.Class;

/// <summary>
/// Serviço responsável pelas operações de gerenciamento de jogos, incluindo cadastro, alteração, compra, consulta e exclusão.
/// </summary>
public class JogosServices(
    IJogosRepository jogosRepository,
    ICarteiraRepository carteiraRepository,
    IBibliotecaRepository bibliotecaRepository,
    IPromocoesServices promocoesServices,
    ILoggerServices logger) : IJogosServices
{
    /// <summary>
    /// Altera os dados de um jogo existente.
    /// </summary>
    /// <param name="Nome">Nome do jogo a ser alterado.</param>
    /// <param name="request">Novos dados do jogo.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo os dados atualizados do jogo (<see cref="JogosDto.JogosDtoResponse"/>)
    /// ou erro caso o jogo não seja encontrado.
    /// </returns>
    public async Task<Result<JogosDto.JogosDtoResponse>> AlterarJogos(string Nome, JogosDto.JogosDtoRequest request) {
        var jogo = await jogosRepository.ObterJogoPorNome(Nome);
        if (jogo == null) {
            await logger.LogError($"Jogo não encontrado.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Jogo não encontrado.", "500");
        }

        jogo.Nome = request.Nome;
        jogo.Descricao = request.Descricao;
        jogo.Preco = request.Preco;
        jogo.Tipo = request.Tipo;
        jogo.Quantidade = request.Quantidade;
        await jogosRepository.AtualizarJogos(jogo);

        await logger.LogInformation("Finalizou AlterarJogos");
        return Result.Success(new JogosDto.JogosDtoResponse {
            Nome = jogo.Nome,
            Descricao = jogo.Descricao,
            Preco = jogo.Preco,
            Tipo = jogo.Tipo,
            Quantidade = jogo.Quantidade
        });
    }

    /// <summary>
    /// Cadastra um novo jogo no sistema.
    /// </summary>
    /// <param name="request">Dados do jogo a ser cadastrado.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo os dados do jogo cadastrado (<see cref="JogosDto.JogosDtoResponse"/>)
    /// ou erro caso o jogo já exista.
    /// </returns>
    public async Task<Result<JogosDto.JogosDtoResponse>> CadastrarJogos(JogosDto.JogosDtoRequest request) {

        var jogo = await jogosRepository.ObterJogoPorNome(request.Nome);
        if (jogo != null) {
            await logger.LogError($"Jogo já foi cadastrado.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Esse jogo já foi cadastrado.", "500");
        }

        jogo = JogosEntity.Criar(request.Nome,
           request.Descricao,
           request.Preco,
           request.Tipo,
           request.Quantidade,
           true);

        await jogosRepository.AdicionarJogos(jogo);

        await logger.LogInformation("Finalizou CadastrarJogos");
        return Result.Success(new JogosDto.JogosDtoResponse {
            Nome = jogo.Nome,
            Descricao = jogo.Descricao,
            Preco = jogo.Preco,
            Tipo = jogo.Tipo,
            Quantidade = jogo.Quantidade
        });

    }

    /// <summary>
    /// Realiza a compra de um jogo pelo usuário.
    /// </summary>
    /// <param name="request">Dados do jogo a ser comprado.</param>
    /// <param name="userId">Identificador do usuário (string GUID).</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo os dados do jogo comprado (<see cref="JogosDto.JogosDtoResponse"/>)
    /// ou erro caso haja saldo insuficiente, jogo indisponível ou o usuário já possua o jogo.
    /// </returns>
    public async Task<Result<JogosDto.JogosDtoResponse>> ComprarJogos(JogosDto.JogosDtoComprarJogos request, string userId) {

        if (string.IsNullOrEmpty(userId)) {
            await logger.LogError($"Token ausente ou inválido.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Token ausente ou inválido.", "401");
        }

        var jogo = await jogosRepository.ObterJogoPorNome(request.Nome);
        if (jogo == null) {
            await logger.LogError($"Jogo não encontrado.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Jogo não encontrado.", "500");
        }

        if (jogo.Quantidade <= 0) {
            await logger.LogError($"Esse jogo não está disponível.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Esse jogo não está disponível.", "500");
        }

        var jaPossui = await jogosRepository.VerificaSeUsuarioPossuiJogo(jogo.Id.ToString(), userId);
        if (jaPossui != null) {
            await logger.LogError($"Você ja possui esse jogo, não é possivel comprar novamente.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Você ja possui esse jogo, não é possivel comprar novamente.", "500");
        }

        var promoResult = await promocoesServices.ConsultaPromocoesAtivas();
        var precoFinal = jogo.Preco;

        if (promoResult.IsSuccess) {
            var promocao = promoResult.Value;

            if (promocao.IdJogos.Contains(jogo.Id)) {
                precoFinal = promocao.Valor;
            }
        }

        var carteira = await carteiraRepository.ObtemSaldoPorId(Guid.Parse(userId));
        if (carteira == null || carteira.Saldo < precoFinal) {
            await logger.LogError($"Saldo insuficiente.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Saldo insuficiente.", "500");
        }

        var novaCarteira = CarteiraEntity.Atualizar(
            carteira.Id,
            carteira.UsuarioId,
            carteira.Saldo - precoFinal);

        carteiraRepository.AlteraSaldo(novaCarteira);

        jogo.Quantidade -= 1;
        await jogosRepository.AtualizarJogos(jogo);

        await bibliotecaRepository.AdicionaNaBiblioteca(
            BibliotecaEntity.Criar(jogo.Id, Guid.Parse(userId)));

        var response = new JogosDto.JogosDtoResponse {
            Nome = jogo.Nome,
            Descricao = jogo.Descricao,
            Preco = precoFinal,
            Tipo = jogo.Tipo,
            Quantidade = jogo.Quantidade
        };

        await logger.LogInformation("Finalizou ComprarJogos");

        return Result.Success(response);
    }

    /// <summary>
    /// Consulta todos os jogos cadastrados no sistema.
    /// </summary>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo a lista de jogos (<see cref="JogosDto.JogosDtoResponse"/>)
    /// ou erro caso não haja jogos disponíveis.
    /// </returns>
    public async Task<Result<List<JogosDto.JogosDtoResponse>>> ConsultarJogos() {
        var jogos = await jogosRepository.ObterJogos();

        if (jogos == null || !jogos.Any()) {
            await logger.LogError($"Essa plataforma não possui jogos disponíveis.");
            return Result.Failure<List<JogosDto.JogosDtoResponse>>("Essa plataforma não possui jogos disponíveis.", "500");
        }

        await logger.LogInformation("Finalizou ConsultarJogos");
        var response = jogos.Select(x => new JogosDto.JogosDtoResponse {
            Nome = x.Nome,
            Descricao = x.Descricao,
            Preco = x.Preco,
            Tipo = x.Tipo,
            Quantidade = x.Quantidade
        }).ToList();

        return Result.Success(response);
    }

    /// <summary>
    /// Consulta os dados de um jogo específico pelo nome.
    /// </summary>
    /// <param name="Nome">Nome do jogo a ser consultado.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo os dados do jogo (<see cref="JogosDto.JogosDtoResponse"/>)
    /// ou erro caso o jogo não seja encontrado.
    /// </returns>
    public async Task<Result<JogosDto.JogosDtoResponse>> ConsultarUnicoJogo(string Nome) {
        var jogo = await jogosRepository.ObterJogoPorNome(Nome);
        if (jogo == null) {
            await logger.LogError($"Jogo não encontrado.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Jogo não encontrado.", "500");
        }

        await logger.LogInformation("Finalizou ConsultarUnicoJogo");
        return Result.Success(new JogosDto.JogosDtoResponse {
            Nome = jogo.Nome,
            Descricao = jogo.Descricao,
            Preco = jogo.Preco,
            Tipo = jogo.Tipo,
            Quantidade = jogo.Quantidade
        });
    }

    /// <summary>
    /// Deleta (desativa) um jogo do sistema pelo nome.
    /// </summary>
    /// <param name="Nome">Nome do jogo a ser deletado.</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo os dados do jogo desativado (<see cref="JogosDto.JogosDtoResponse"/>)
    /// ou erro caso o jogo não seja encontrado.
    /// </returns>
    public async Task<Result<JogosDto.JogosDtoResponse>> DeletarJogos(string Nome) {
        var jogo = await jogosRepository.ObterJogoPorNome(Nome);
        if (jogo == null) {
            await logger.LogError($"Jogo não encontrado.");
            return Result.Failure<JogosDto.JogosDtoResponse>("Jogo não encontrado.", "500");
        }

        jogo.Ativo = false;
        await jogosRepository.AtualizarJogos(jogo);

        await logger.LogInformation("Finalizou DeletarJogos");
        return Result.Success(new JogosDto.JogosDtoResponse {
            Nome = jogo.Nome,
            Descricao = jogo.Descricao,
            Preco = jogo.Preco,
            Tipo = jogo.Tipo,
            Quantidade = jogo.Quantidade
        });
    }
}