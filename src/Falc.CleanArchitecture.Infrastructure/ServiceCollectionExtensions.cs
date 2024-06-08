using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Infrastructure;

/// <summary>
/// Service collection extension methods that related to infrastructure.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure and register services relating to infrastructure.
    /// </summary>
    /// <param name="serviceCollection">Service collection to which services are registered.</param>
    /// <param name="configuratorAction">Choose how to configure your infrastructure layer.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection, 
        Action<Configurator> configuratorAction)
    {
        var configurator = new Configurator(serviceCollection);
        configuratorAction.Invoke(configurator);
        
        return serviceCollection;
    }
}