using MediatR;
using VerbundPflegehilfe.Domain.Common;
using Microsoft.EntityFrameworkCore;
using VerbundPflegehilfe.Domain.Entities;
using VerbundPflegehilfe.Application.Common.Interfaces;

namespace VerbundPflegehilfe.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : DbContext(options), IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedAt = DateTime.UtcNow;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(cancellationToken);

        return result;
    }

    private async Task DispatchEvents(CancellationToken cancellationToken)
    {
        var domainEventEntities = ChangeTracker.Entries<BaseAuditableEntity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .Select(x => x.Entity)
            .ToList();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TodoItem>().Property(t => t.Title).HasMaxLength(200).IsRequired();
    }
}