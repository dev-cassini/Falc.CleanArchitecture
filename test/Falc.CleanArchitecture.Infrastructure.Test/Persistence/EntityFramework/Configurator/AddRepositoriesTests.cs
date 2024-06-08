using System.Reflection;
using Falc.CleanArchitecture.Infrastructure.Persistence.EntityFramework.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Falc.CleanArchitecture.Infrastructure.Test.Persistence.EntityFramework.Configurator;

[TestFixture]
public class AddRepositoriesTests
{
    [Test]
    public void WhenAddRepositoriesIsCalled_ThenAllEfRepositoriesAreRegistered()
    {
        var serviceCollection = new ServiceCollection()
            .AddInfrastructure(infrastructureConfigurator =>
            {
                infrastructureConfigurator.AddPersistence(persistenceConfigurator =>
                {
                    persistenceConfigurator.AddEntityFramework(efConfigurator =>
                    {
                        efConfigurator.AddRepositories(Assembly.GetExecutingAssembly());
                    });
                });
            });

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        Assert.Multiple(() =>
        {
            Assert.That(serviceProvider.GetServices<ITestEfRepository>(), Is.Not.Null);
            Assert.That(serviceProvider.GetServices<IAnotherTestEfRepository>(), Is.Not.Null);
        });
    }

    private interface ITestEfRepository : IEfRepository { }
    private class TestEfRepository : ITestEfRepository { }
    private interface IAnotherTestEfRepository : IEfRepository { }
    private class AnotherTestEfRepository : IAnotherTestEfRepository { }
}