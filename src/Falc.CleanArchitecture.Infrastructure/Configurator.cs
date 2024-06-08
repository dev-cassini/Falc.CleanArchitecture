using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Infrastructure;

/// <summary>
/// Configure infrastructure implementation.
/// </summary>
/// <param name="serviceCollection">Service collection to which services are registered.</param>
public class Configurator(IServiceCollection serviceCollection)
{
    /// <summary>
    /// Configure and register services relating to persistence.
    /// </summary>
    /// <param name="configuratorAction">Choose how to configure your persistence layer.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public Configurator AddPersistence(Action<Persistence.Configurator> configuratorAction)
    {
        var configurator = new Persistence.Configurator(serviceCollection);
        configuratorAction.Invoke(configurator);
        
        return this;
    }
}