using System.Reflection;
using Falc.CleanArchitecture.Application.Services;
using Falc.CleanArchitecture.Domain;
using Falc.CleanArchitecture.Domain.Services;
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
                .Where(x => x != typeof(IRepository))
                .Where(x => x != typeof(IEfRepository))
                .Single();

            serviceCollection.AddScoped(serviceType, efRepositoryType);
        }

        return this;
    }

    /// <summary>
    /// Register service that handles creating and committing database transactions.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public Configurator AddTransactionService<T>() where T : DbContext
    {
        serviceCollection.AddScoped<ITransactionService, EfTransactionService<T>>();
        return this;
    }
    
    /// <summary>
    /// Register service that handles getting and clearing domain events from aggregate roots
    /// using entity framework's change tracker.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public Configurator AddDomainEventService<T>() where T : DbContext
    {
        serviceCollection.AddScoped<IDomainEventService, EfDomainEventService<T>>();
        return this;
    }
}