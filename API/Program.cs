using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using Modules;
using API;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PlannerDb") ?? "Data Source=PlannerDb.db";

builder.Services.AddSqlite<PlannerDb>(connectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddOpenApi();

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference("/docs", options =>
{
    // Optional: Customize the title and layout
    options.Title = "My API Reference";
    options.Layout = ScalarLayout.Classic;
});

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

#region Users
var userItems = app.MapGroup("/Users");

userItems.MapGet("/", UserAPI.GetAllUsers);
userItems.MapGet("/{id}", UserAPI.GetUserById);
userItems.MapPost("/", UserAPI.CreateUser);
userItems.MapPut("/{id}", UserAPI.UpdateUser);
userItems.MapDelete("/{id}", UserAPI.DeleteUser);
#endregion

#region Project Users
var projectUserItems = app.MapGroup("/ProjectUsers");

projectUserItems.MapGet("/{id}", ProjectUserAPI.GetProjectUsers);
projectUserItems.MapPost("/", ProjectUserAPI.CreateProjectUser);
projectUserItems.MapPut("/{id}", ProjectUserAPI.UpdateProjectUserRole);
projectUserItems.MapDelete("/{id}", ProjectUserAPI.DeleteProjectUser);
#endregion

#region Task Assignees
var taskAssigneeItems = app.MapGroup("/TaskAssignees");

taskAssigneeItems.MapGet("/{id}", TaskAssigneeAPI.GetTaskAssignees);
taskAssigneeItems.MapPost("/", TaskAssigneeAPI.AddTaskAssignee);
taskAssigneeItems.MapDelete("/{id}", TaskAssigneeAPI.DeleteTaskAssignee);
#endregion

app.Run();