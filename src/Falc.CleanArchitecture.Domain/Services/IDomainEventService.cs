using MediatR;

namespace Falc.CleanArchitecture.Domain.Services;

public interface IDomainEventService
{
    IReadOnlyList<INotification> Get();
    void ClearAll();
}