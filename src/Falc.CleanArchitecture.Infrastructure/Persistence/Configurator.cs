using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Infrastructure.Persistence;

/// <summary>
/// Configure persistence implementation.
/// </summary>
/// <param name="serviceCollection">Service collection to which services are registered.</param>
public class Configurator(IServiceCollection serviceCollection)
{
    /// <summary>
    /// Configure and register services relating to Entity Framework.
    /// </summary>
    /// <param name="configuratorAction">Choose how to configure Entity Framework.</param>
    /// <returns>The same configurator.</returns>
    public Configurator AddEntityFramework(Action<EntityFramework.Configurator> configuratorAction)
    {
        var configurator = new EntityFramework.Configurator(serviceCollection);
        configuratorAction.Invoke(configurator);
        
        return this;
    }
}