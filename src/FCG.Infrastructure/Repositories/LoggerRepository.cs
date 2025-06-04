using BCrypt.Net;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.Data;

namespace FCG.Infrastructure.Repositories;

public class LoggerRepository : ILoggerRepository
{
    private readonly DbFcg _dbFcg;

    /// <summary>
    /// Construtor do repositório LoggerRepository.
    /// </summary>
    /// <param name="DbFcg">Contexto do banco de dados.</param>
    public LoggerRepository(DbFcg dbFcg) {
        _dbFcg = dbFcg;
    }

    /// <summary>
    /// Confirma as transações no banco de dados.
    /// </summary>
    public async Task Commit() => await _dbFcg.SaveChangesAsync();

    /// <summary>
    /// Adiciona um log de erro ao repositório.
    /// </summary>
    /// <param name="loggerEnt">Entidade que representa o log de erro.</param>
    public async Task AddILogger(ILoggerEnt loggerEnt) => await _dbFcg.LOGS.AddAsync(loggerEnt);
}

