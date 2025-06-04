using FCG.Domain.Shared.DTO;
using FCG.Domain.Shared.Results;

namespace FCG.Domain.Services.Interface;

public interface IPromocoesServices
{
    Task<Result<PromocoesDto.PromocoesDtoResponse>> InserePromocoes(PromocoesDto.PromocoesDtoRequest request);
    Task<Result<PromocoesDto.PromocoesDtoResponse>> ConsultaPromocoesAtivas();
}
