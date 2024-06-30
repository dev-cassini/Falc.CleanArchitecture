using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Application;

/// <summary>
/// Service collection extension methods that relate to the application layer.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure and register services relating to the application layer.
    /// </summary>
    /// <param name="serviceCollection">Service collection to which services are registered.</param>
    /// <param name="configuratorAction">Choose how to configure your application layer.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddApplication(
        this IServiceCollection serviceCollection, 
        Action<Configurator> configuratorAction)
    {
        var configurator = new Configurator(serviceCollection);
        configuratorAction.Invoke(configurator);
        
        return serviceCollection;
    }
}