using System.Reflection;
using Falc.CleanArchitecture.Domain;
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
            Assert.That(serviceProvider.GetService<ITestRepository>(), Is.Not.Null);
            Assert.That(serviceProvider.GetService<IAnotherTestRepository>(), Is.Not.Null);
            Assert.That(serviceProvider.GetService<INotAnEfRepository>(), Is.Null);
        });
    }

    private interface ITestRepository : IRepository;
    private class TestEfRepository : ITestRepository, IEfRepository;
    private interface IAnotherTestRepository : IRepository;
    private class AnotherTestEfRepository : IAnotherTestRepository, IEfRepository;
    private interface INotAnEfRepository : IRepository;
    private class NotAnEfRepository : INotAnEfRepository;
}