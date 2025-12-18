using Microsoft.EntityFrameworkCore;
using Modules;

internal static class InMemoryDbFactory
{
    // Create new options for an in-memory database with a unique name per test class/run
    internal static DbContextOptions<PlannerDb> CreateNewContextOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<PlannerDb>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;
    }

    // Helper to create and seed a context for a test
    internal static PlannerDb CreateAndSeedContext(string databaseName)
    {
        var options = CreateNewContextOptions(databaseName);
        var context = new PlannerDb(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Use this line to add seed data as needed
        //context.Books.AddRange();
        context.SaveChanges();

        return context;
    }
}