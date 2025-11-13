using Microsoft.EntityFrameworkCore;
using Modules;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PlannerTaskDb>(opt => opt.UseInMemoryDatabase("TaskList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/Taskitems", async (PlannerTaskDb db) =>
    await db.PlannerTask.ToListAsync());

app.MapGet("/Taskitems/{id}", async (int id, PlannerTaskDb db) =>
    await db.PlannerTask.FindAsync(id)
        is PlannerTask Task
            ? Results.Ok(Task)
            : Results.NotFound());

app.MapPost("/Taskitems", async (PlannerTask PlannerTask, PlannerTaskDb db) =>
{
    db.PlannerTask.Add(PlannerTask);
    await db.SaveChangesAsync();

    return Results.Created($"/Taskitems/{PlannerTask.Id}", PlannerTask);
});

app.MapPut("/Taskitems/{id}", async (int id, PlannerTask inputTask, PlannerTaskDb db) =>
{
    var Task = await db.PlannerTask.FindAsync(id);

    if (Task is null) return Results.NotFound();

    Task.Name = inputTask.Name;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/Taskitems/{id}", async (int id, PlannerTaskDb db) =>
{
    if (await db.PlannerTask.FindAsync(id) is PlannerTask PlannerTask)
    {
        db.PlannerTask.Remove(PlannerTask);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();