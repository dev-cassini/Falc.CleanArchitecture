using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Application.Messaging.MediatR.PipelineBehaviours;

public class Configurator
{
    private readonly MediatRServiceConfiguration _mediatRServiceConfiguration;

    public Configurator(MediatRServiceConfiguration mediatRServiceConfiguration)
    {
        _mediatRServiceConfiguration = mediatRServiceConfiguration;
    }

    public Configurator AddUnitOfWorkPipelineWrapper()
    {
        _mediatRServiceConfiguration.AddOpenBehavior(typeof(UnitOfWorkPipelineBehaviour<,>));
        return this;
    }
}