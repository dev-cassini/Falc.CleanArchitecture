using Falc.CleanArchitecture.Domain.Services;
using MediatR;

namespace Falc.CleanArchitecture.Application.Messaging.MediatR;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(
        this IMediator mediator, 
        IDomainEventService domainEventService,
        CancellationToken cancellationToken)
    {
        var domainEvents = domainEventService.Get();
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
        
        domainEventService.ClearAll();
    }
}