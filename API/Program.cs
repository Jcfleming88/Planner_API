using API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Modules;
using Scalar.AspNetCore;
using Auth0.AspNetCore.Authentication.Api;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PlannerDb") ?? "Data Source=PlannerDb.db";

builder.Services.AddSqlite<PlannerDb>(connectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddOpenApi();
builder.Services.AddMvc();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddAuth0ApiAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.JwtBearerOptions = new JwtBearerOptions
    {
        Audience = builder.Configuration["Auth0:Audience"]
    };
});
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4040") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
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

taskItems.MapGet("/", PlannerTaskAPI.GetAllTasks).RequireAuthorization();
taskItems.MapGet("/project/{projectId}", PlannerTaskAPI.GetTasksByProjectId).RequireAuthorization();
taskItems.MapGet("/{id}", PlannerTaskAPI.GetTaskById).RequireAuthorization();
taskItems.MapPost("/", PlannerTaskAPI.CreateTask).RequireAuthorization();
taskItems.MapPut("/{id}", PlannerTaskAPI.UpdateTask).RequireAuthorization();
taskItems.MapDelete("/{id}", PlannerTaskAPI.DeleteTask).RequireAuthorization();
#endregion

#region Project calls
var projectItems = app.MapGroup("/Projects");

projectItems.MapGet("/", ProjectAPI.GetAllProjects).RequireAuthorization();
projectItems.MapGet("/{id}", ProjectAPI.GetProjectById).RequireAuthorization();
projectItems.MapPost("/", ProjectAPI.CreateProject).RequireAuthorization();
projectItems.MapPut("/{id}", ProjectAPI.UpdateProject).RequireAuthorization();
projectItems.MapDelete("/{id}", ProjectAPI.DeleteProject).RequireAuthorization();
#endregion

#region Users
var userItems = app.MapGroup("/Users");

userItems.MapGet("/", UserAPI.GetAllUsers).RequireAuthorization();
userItems.MapGet("/{id}", UserAPI.GetUserById).RequireAuthorization();
userItems.MapPost("/", UserAPI.CreateUser).RequireAuthorization();
userItems.MapPut("/{id}", UserAPI.UpdateUser).RequireAuthorization();
userItems.MapDelete("/{id}", UserAPI.DeleteUser).RequireAuthorization();
#endregion

#region Project Users
var projectUserItems = app.MapGroup("/ProjectUsers");

projectUserItems.MapGet("/{id}", ProjectUserAPI.GetProjectUsers).RequireAuthorization();
projectUserItems.MapPost("/", ProjectUserAPI.CreateProjectUser).RequireAuthorization();
projectUserItems.MapPut("/{id}", ProjectUserAPI.UpdateProjectUserRole).RequireAuthorization();
projectUserItems.MapDelete("/{id}", ProjectUserAPI.DeleteProjectUser).RequireAuthorization();
#endregion

#region Task Assignees
var taskAssigneeItems = app.MapGroup("/TaskAssignees");

taskAssigneeItems.MapGet("/{id}", TaskAssigneeAPI.GetTaskAssignees).RequireAuthorization();
taskAssigneeItems.MapPost("/", TaskAssigneeAPI.AddTaskAssignee).RequireAuthorization();
taskAssigneeItems.MapDelete("/{id}", TaskAssigneeAPI.DeleteTaskAssignee).RequireAuthorization();
#endregion

app.Run();