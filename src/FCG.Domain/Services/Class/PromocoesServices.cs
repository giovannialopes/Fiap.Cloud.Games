﻿using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Services.Interface;
using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace FCG.Domain.Services.Class;

/// <summary>
/// Serviço responsável pelo gerenciamento de promoções, incluindo cadastro e consulta de promoções ativas.
/// </summary>
public class PromocoesServices(IPromocoesRepository promocoesRepository, ILoggerServices logger) : IPromocoesServices
{
    /// <summary>
    /// Adiciona uma nova promoção ao sistema.
    /// </summary>
    /// <param name="request">Dados da promoção a ser cadastrada (nome, valor, datas e jogos associados).</param>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo os dados da promoção cadastrada (<see cref="PromocoesDto.PromocoesDtoResponse"/>)
    /// ou erro caso ocorra alguma falha na operação.
    /// </returns>
    public async Task<Result<PromocoesDto.PromocoesDtoResponse>> InserePromocoes(PromocoesDto.PromocoesDtoRequest request) {
        var promocoes = PromocoesEntity.Criar(
            request.Nome,
            request.Valor,
            request.DataInicio,
            request.DataFim,
            request.IdJogos);

        await promocoesRepository.AdicionarPromocoes(promocoes);

        await logger.LogInformation("Finalizou InserePromocoes");
        return Result.Success(new PromocoesDto.PromocoesDtoResponse {
            Nome = promocoes.Nome,
            Valor = promocoes.Valor,
            DataInicio = promocoes.DataInicio,
            DataFim = promocoes.DataFim,
            IdJogos = promocoes.IdJogos
        });
    }

    /// <summary>
    /// Consulta a promoção ativa no sistema.
    /// </summary>
    /// <returns>
    /// Um <see cref="Result{T}"/> contendo os dados da promoção ativa (<see cref="PromocoesDto.PromocoesDtoResponse"/>)
    /// ou erro caso nenhuma promoção esteja ativa.
    /// </returns>
    public async Task<Result<PromocoesDto.PromocoesDtoResponse>> ConsultaPromocoesAtivas() {
        var promocoes = await promocoesRepository.ConsultaPromocoesAtivas();
        if (promocoes == null) {
            await logger.LogError($"Nenhuma promoção ativa encontrada.");
            return Result.Failure<PromocoesDto.PromocoesDtoResponse>("Nenhuma promoção ativa encontrada.", "500");
        }

        await logger.LogInformation("Finalizou ConsultaPromocoesAtivas");
        return Result.Success(new PromocoesDto.PromocoesDtoResponse {
            Nome = promocoes.Nome,
            Valor = promocoes.Valor,
            DataInicio = promocoes.DataInicio,
            DataFim = promocoes.DataFim,
            IdJogos = promocoes.IdJogos
        });
    }
}