using Microsoft.EntityFrameworkCore;
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
    options.Layout = ScalarLayout.Modern;
    options.HeadContent = """
        <div>
            <style>
                .custom-header {
                    background-color: #421AE5;
                    border-bottom: 5px solid #eaecee;
                    color: #eaecee;
                    font-family: Arial, Helvetica, sans-serif;
                    padding: 8px;
                    text-align: center;
                }
            </style>
            <div class="custom-header">
                <h1>Planner API Docs</h1>
                <h3>
                    Welcome to the Planner API documentation. Here you will find all the information you need to interact with the Planner API.
                </h3>
            </div>
        </div>

    """;
});

#region Ping
// Basic response to get a ping back from the API
app.MapGet("/", PingAPI.Ping);
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