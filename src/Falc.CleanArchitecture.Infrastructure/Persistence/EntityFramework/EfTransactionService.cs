using Falc.CleanArchitecture.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Falc.CleanArchitecture.Infrastructure.Persistence.EntityFramework;

public class EfTransactionService<T>(T dbContext) : ITransactionService, IDisposable, IAsyncDisposable
    where T : DbContext
{
    private IDbContextTransaction? _transaction;
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(cancellationToken);
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null) await _transaction.DisposeAsync();
        await dbContext.DisposeAsync();
    }
}