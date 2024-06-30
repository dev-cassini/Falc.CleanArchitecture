using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Application.Messaging.MediatR;

public static class MediatRServiceConfigurationExtensions
{
    public static MediatRServiceConfiguration ConfigurePipeline(
        this MediatRServiceConfiguration mediatRServiceConfiguration,
        Action<PipelineBehaviours.Configurator> configuratorAction)
    {
        var configurator = new PipelineBehaviours.Configurator(mediatRServiceConfiguration);
        configuratorAction.Invoke(configurator);

        return mediatRServiceConfiguration;
    }
}