using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Modules;
using API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PlannerTaskDb>(opt => opt.UseInMemoryDatabase("Planner"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", async () =>
{
    return new Ping();
});

var taskItems = app.MapGroup("/Taskitems");

taskItems.MapGet("/", PlannerTaskAPI.GetAllTasks);
taskItems.MapGet("/project/{projectId}", PlannerTaskAPI.GetTasksByProjectId);
taskItems.MapGet("/{id}", PlannerTaskAPI.GetTaskById);
taskItems.MapPost("/", PlannerTaskAPI.CreateTask);
taskItems.MapPut("/{id}", PlannerTaskAPI.UpdateTask);
taskItems.MapDelete("/{id}", PlannerTaskAPI.DeleteTask);

app.Run();