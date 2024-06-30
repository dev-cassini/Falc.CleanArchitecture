using Falc.CleanArchitecture.Domain;
using Falc.CleanArchitecture.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Falc.CleanArchitecture.Infrastructure.Persistence.EntityFramework;

public class EfDomainEventService<T>(T dbContext) : IDomainEventService 
    where T : DbContext
{
    public IReadOnlyList<INotification> Get()
    {
        return dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(x => x.Entity.DomainEvents).ToList();
    }

    public void ClearAll()
    {
        var updatedAggregateRoots = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity).ToList();

        foreach (var updatedAggregateRoot in updatedAggregateRoots)
        {
            updatedAggregateRoot.ClearAllDomainEvent();
        }
    }
}