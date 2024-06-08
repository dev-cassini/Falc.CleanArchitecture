using System.Reflection;
using Falc.CleanArchitecture.Infrastructure.Persistence.EntityFramework.Abstractions;
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

    /// <summary>
    /// Register implementations of <see cref="IEfRepository"/>.
    /// </summary>
    /// <param name="assembly">Assembly in which to search for implementations.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public Configurator AddRepositories(Assembly assembly)
    {
        var efRepositoryTypes = assembly.GetTypes()
            .Where(x => x is { IsAbstract: false, IsInterface: false })
            .Where(x => x.GetInterfaces().Any(y => y == typeof(IEfRepository)));

        foreach (var efRepositoryType in efRepositoryTypes)
        {
            var serviceType = efRepositoryType.GetInterfaces()
                .Single(x => x.GetInterfaces().Any(y => y == typeof(IEfRepository)));

            serviceCollection.AddScoped(serviceType, efRepositoryType);
        }

        return this;
    }
}