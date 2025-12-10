using Microsoft.EntityFrameworkCore;
using Modules;

public static class InMemoryDbFactory
{
    // Create new options for an in-memory database with a unique name per test class/run
    public static DbContextOptions<PlannerDb> CreateNewContextOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<PlannerDb>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;
    }

    // Helper to create and seed a context for a test
    public static PlannerDb CreateAndSeedContext(string databaseName)
    {
        var options = CreateNewContextOptions(databaseName);
        var context = new PlannerDb(options);

        context.Database.EnsureDeleted(); // Start fresh
        context.Database.EnsureCreated();

        // Use this line to add seed data as needed
        //context.Books.AddRange();
        context.SaveChanges();

        return context;
    }
}