using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Application;

/// <summary>
/// Configure application layer.
/// </summary>
/// <param name="serviceCollection">Service collection to which services are registered.</param>
public class Configurator(IServiceCollection serviceCollection)
{
    /// <summary>
    /// Configure and register services relating to MediatR.
    /// </summary>
    /// <param name="configuratorAction">Choose how to configure MediatR.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public Configurator AddMediatR(Action<MediatRServiceConfiguration> configuratorAction)
    {
        var configurator = new MediatRServiceConfiguration();
        configuratorAction.Invoke(configurator);
        
        return this;
    }
}