using Microsoft.EntityFrameworkCore;
using VerbundPflegehilfe.Domain.Entities;

namespace VerbundPflegehilfe.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}