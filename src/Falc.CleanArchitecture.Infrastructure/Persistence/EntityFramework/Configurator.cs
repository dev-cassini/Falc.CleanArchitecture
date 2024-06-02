using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Infrastructure.Persistence.EntityFramework;

/// <summary>
/// Configure Entity Framework implementation.
/// </summary>
/// <param name="serviceCollection">Service collection to which services are registered.</param>
public class Configurator(IServiceCollection serviceCollection)
{
    public Configurator AddDbContext<T>(
        Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction = null,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) 
        where T : DbContext
    {
        serviceCollection.AddDbContext<T>(optionsAction, contextLifetime, optionsLifetime);
        
        return this;
    }
}