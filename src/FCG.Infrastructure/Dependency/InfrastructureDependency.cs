using FCG.Domain.Repositories;
using FCG.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FCG.Infrastructure.Dependency;

public static class InfrastructureDependency
{
    public static IServiceCollection AddRepositories(this IServiceCollection services) {
        return services
            .AddScoped<IPerfilRepository, PerfilRepository>()
            .AddScoped<IJogosRepository, JogosRepository>()
            .AddScoped<IBibliotecaRepository, BibliotecaRepository>()
            .AddScoped<IPromocoesRepository, PromocoesRepository>()
            .AddScoped<ICarteiraRepository, CarteiraRepository>()
            .AddScoped<ILoggerRepository, LoggerRepository>();

    }

}
