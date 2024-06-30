using Falc.CleanArchitecture.Application;
using Falc.CleanArchitecture.Application.Messaging.MediatR;
using Falc.CleanArchitecture.Infrastructure;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Test.Component;

[TestFixture]
public class Tests
{
    [Test]
    public async Task Test1()
    {
        var sqliteConnection = new SqliteConnection("DataSource=:memory:");
        var serviceCollection = new ServiceCollection()
            .AddApplication(configurator =>
            {
                configurator.AddMediatR(mediatRConfigurator =>
                {
                    MediatRServiceConfigurationExtensions.ConfigurePipeline(mediatRConfigurator
                            .RegisterServicesFromAssembly(typeof(Marker).Assembly), pipelineConfigurator =>
                        {
                            pipelineConfigurator.AddUnitOfWorkPipelineWrapper();
                        });
                });
            }).AddInfrastructure(configurator =>
            {
                configurator.AddPersistence(persistenceConfigurator =>
                {
                    persistenceConfigurator.AddEntityFramework(entityFrameworkConfigurator =>
                    {
                        entityFrameworkConfigurator
                            .AddDbContext<TestDbContext>((_, dbContextConfigurator) =>
                            {
                                dbContextConfigurator
                                    .UseSqlite(sqliteConnection)
                                    .EnableSensitiveDataLogging();
                            })
                            .AddRepositories(typeof(Marker).Assembly)
                            .AddTransactionService<TestDbContext>()
                            .AddDomainEventService<TestDbContext>();
                    });
                });
            });

        var serviceProvider = serviceCollection.BuildServiceProvider();
        sqliteConnection.Open();
        
        var dbContext = serviceProvider.GetRequiredService<TestDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        var testAggregateRoot = new TestAggregateRoot(Guid.NewGuid(), "Name");
        await dbContext.TestAggregateRoots.AddAsync(testAggregateRoot);
        await dbContext.SaveChangesAsync();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var command = new UpdateTestAggregateRootCommand(testAggregateRoot.Id, "Updated Name");
        await mediator.Send(command);
    }
}