namespace Falc.CleanArchitecture.Application.Services;

public interface ITransactionService
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
}