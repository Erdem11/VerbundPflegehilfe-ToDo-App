using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VerbundPflegehilfe.Domain.Entities;

namespace VerbundPflegehilfe.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
{
    public async Task InitialiseAsync()
    {
        try
        {
            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (context.TodoItems.Any())
        {
            return; 
        }

        context.TodoItems.AddRange(
        new TodoItem("Learn Clean Architecture", DateTime.Now.AddDays(1)),
        new TodoItem("Submit the Case Study", DateTime.Now.AddDays(2)),
        new TodoItem("This is an Overdue Task", DateTime.Now.AddDays(-1))
        );

        await context.SaveChangesAsync();
    }
}