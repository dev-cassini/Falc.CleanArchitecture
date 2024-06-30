using Falc.CleanArchitecture.Domain;
using Falc.CleanArchitecture.Infrastructure.Persistence.EntityFramework.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falc.CleanArchitecture.Application.Test.Component;

public class TestDbContext : DbContext
{
    public DbSet<TestAggregateRoot> TestAggregateRoots { get; set; } = null!;
    
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TestAggregateRootConfiguration());
        
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties().Where(x => x.IsPrimaryKey()))
            {
                property.ValueGenerated = ValueGenerated.Never;
            }
        }
    }
}

public class TestAggregateRootConfiguration : IEntityTypeConfiguration<TestAggregateRoot> 
{
    public void Configure(EntityTypeBuilder<TestAggregateRoot> builder)
    {
        builder.ToTable(nameof(TestDbContext.TestAggregateRoots));
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name);
    }
}

public interface ITestAggregateRootRepository : IRepository
{
    Task AddAsync(TestAggregateRoot entity);
    Task<TestAggregateRoot> GetAsync(Guid id);
    Task SaveChangesAsync();
}

public class EfTestAggregateRootRepository(TestDbContext dbContext) : ITestAggregateRootRepository, IEfRepository
{
    public async Task AddAsync(TestAggregateRoot entity)
    {
        await dbContext.TestAggregateRoots.AddAsync(entity);
    }

    public async Task<TestAggregateRoot> GetAsync(Guid id)
    {
        return await dbContext.TestAggregateRoots.SingleAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}

public class TestAggregateRoot(Guid id, string name) : AggregateRoot
{
    public Guid Id { get; } = id;
    public string Name { get; set; } = name;

    public void Update(string name)
    {
        Name = name;
        
        AddDomainEvent(new TestAggregateRootUpdated(Id, name));
    }
}

public record TestAggregateRootUpdated(Guid Id, string Name) : INotification;

public record UpdateTestAggregateRootCommand(Guid Id, string Name) : IRequest<Guid>;

public class UpdateTestAggregateRootCommandHandler(ITestAggregateRootRepository repository) : IRequestHandler<UpdateTestAggregateRootCommand, Guid>
{
    public async Task<Guid> Handle(UpdateTestAggregateRootCommand command, CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsync(command.Id);
        entity.Update(command.Name);
        await repository.SaveChangesAsync();
        
        return entity.Id;
    }
}

public class TestAggregateRootUpdatedHandler : INotificationHandler<TestAggregateRootUpdated>
{
    public async Task Handle(TestAggregateRootUpdated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Test aggregate root {notification.Id} name updated to {notification.Name}.");
    }
}