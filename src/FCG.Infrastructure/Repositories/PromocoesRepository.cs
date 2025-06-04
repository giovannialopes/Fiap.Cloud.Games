using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public class PromocoesRepository : IPromocoesRepository
{
    private readonly DbFcg _dbFcg;

    public PromocoesRepository(DbFcg dbFcg) {
        _dbFcg = dbFcg;
    }
    public async Task Commit() => await _dbFcg.SaveChangesAsync();


    public async Task AdicionarPromocoes(PromocoesEntity promocoes) {
        await _dbFcg.PROMOCOES.AddAsync(promocoes);
        await Commit();
    }

    public async Task<PromocoesEntity> ConsultaPromocoesAtivas() {
        var agora = DateTime.Now;

        var promocao = await _dbFcg.PROMOCOES
            .Where(p => p.DataInicio <= agora && p.DataFim >= agora)
            .OrderBy(p => p.DataInicio) 
            .FirstOrDefaultAsync();

        if (promocao == null)
            return null;

        return new PromocoesEntity {
            Id = promocao.Id,
            Nome = promocao.Nome,
            Valor = promocao.Valor,
            DataInicio = promocao.DataInicio,
            DataFim = promocao.DataFim,
            IdJogos = promocao.IdJogos
        };
    }
}
