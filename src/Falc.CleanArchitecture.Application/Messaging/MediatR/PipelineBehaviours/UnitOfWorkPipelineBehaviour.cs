using Falc.CleanArchitecture.Application.Services;
using Falc.CleanArchitecture.Domain.Services;
using MediatR;

namespace Falc.CleanArchitecture.Application.Messaging.MediatR.PipelineBehaviours;

public class UnitOfWorkPipelineBehaviour<TRequest, TResponse>(
    ITransactionService transactionService,
    IDomainEventService domainEventService,
    IMediator mediator)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await transactionService.BeginTransactionAsync(cancellationToken);
        var response = await next();

        await mediator.DispatchDomainEventsAsync(domainEventService, cancellationToken);
        await transactionService.CommitTransactionAsync(cancellationToken);

        return response;
    }
}