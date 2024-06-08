using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Infrastructure.Persistence.EntityFramework;

/// <summary>
/// Service provider extension methods that related to Entity Framework.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Migrate a <see cref="DbContext"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="serviceProvider">Service provider in which the <see cref="DbContext"/> is registered.</param>
    /// <typeparam name="T">Type of <see cref="DbContext"/>.</typeparam>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceProvider MigrateDatabase<T>(this IServiceProvider serviceProvider) 
        where T : DbContext
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        dbContext.Database.Migrate();

        return serviceProvider;
    }
}