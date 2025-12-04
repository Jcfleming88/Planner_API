using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Modules;
using API;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PlannerDb") ?? "Data Source=PlannerDb.db";
builder.Services.AddSqlite<PlannerTaskDb>(connectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

#region Ping
// Basic response to get a ping back from the API
app.MapGet("/", async () =>
{
    return new Ping();
});
#endregion

#region Tasks calls
var taskItems = app.MapGroup("/Taskitems");

taskItems.MapGet("/", PlannerTaskAPI.GetAllTasks);
taskItems.MapGet("/project/{projectId}", PlannerTaskAPI.GetTasksByProjectId);
taskItems.MapGet("/{id}", PlannerTaskAPI.GetTaskById);
taskItems.MapPost("/", PlannerTaskAPI.CreateTask);
taskItems.MapPut("/{id}", PlannerTaskAPI.UpdateTask);
taskItems.MapDelete("/{id}", PlannerTaskAPI.DeleteTask);
#endregion

#region Project calls
var projectItems = app.MapGroup("/Projects");

projectItems.MapGet("/", ProjectAPI.GetAllProjects);
projectItems.MapGet("/{id}", ProjectAPI.GetProjectById);
projectItems.MapPost("/", ProjectAPI.CreateProject);
projectItems.MapPut("/{id}", ProjectAPI.UpdateProject);
projectItems.MapDelete("/{id}", ProjectAPI.DeleteProject);
#endregion

app.Run();