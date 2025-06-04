using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface IPromocoesRepository : ICommit
{
    Task AdicionarPromocoes(PromocoesEntity promocoes);
    Task<PromocoesEntity> ConsultaPromocoesAtivas();
}
