using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface ILoggerRepository : ICommit
{
    Task AddILogger(ILoggerEnt loggerEnt);
}
