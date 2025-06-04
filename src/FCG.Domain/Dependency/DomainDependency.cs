using FCG.Domain.Services.Class;
using FCG.Domain.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace FCG.Domain.Dependency;

public static class DomainDependency 
{
    public static IServiceCollection AddServices(this IServiceCollection services) {
        return services
            .AddScoped<IPerfilServices, PerfilServices>()
            .AddScoped<IJwtServices, JwtServices>()
            .AddScoped<ILoginServices, LoginServices>()
            .AddScoped<ICarteiraServices, CarteiraServices>()
            .AddScoped<IPromocoesServices, PromocoesServices>()
            .AddScoped<IBibliotecaServices, BibliotecaServices>()
            .AddScoped<IJogosServices, JogosServices>()
            .AddScoped<ILoggerServices, LoggerServices>();
    }
}
