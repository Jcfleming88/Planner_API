using System.Collections.Generic;
using System.Linq;
using API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Modules;
using NUnit.Framework;

namespace APITests
{
    public partial class TasksTests
    {
        internal PlannerDb Context { get; set; }
        private List<ProjectDTO> Projects { get; set; } = new List<ProjectDTO>();
        private List<PlannerTaskDTO> Tasks { get; set; } = new List<PlannerTaskDTO>();

        private readonly string User1 = new Guid().ToString();
        private readonly string User2 = new Guid().ToString();

        internal const string DatabaseName = "TasksDb";

        [OneTimeSetUp]
        public void Setup()
        { 
            Context = InMemoryDbFactory.CreateAndSeedContext(DatabaseName);

            // Create some projects to play with
            List<ProjectDTO> projects = new List<ProjectDTO>(){
                new ProjectDTO(new Project(
                    name: "First Project",
                    description: "This is the first project."
                )),
                new ProjectDTO(new Project(
                    name: "Second Project",
                    description: "This is the second project."
                ))
            };

            foreach (ProjectDTO project in projects) 
            {
                // Create a new project in the database
                var result = ProjectAPI.CreateProject(
                    project,
                    Context
                ).GetAwaiter().GetResult();

                // Check the project was created successfully
                if (result == null)
                {
                    Assert.Fail("Setup failed: CreateProject returned null");
                }
                else
                {
                    // Check the project was setup successfully
                    var statusCode = (result as Created<ProjectDTO>)!.StatusCode;
                    Assert.That(statusCode, Is.GreaterThanOrEqualTo(200).And.LessThan(300),
                        $"Setup failed with Status Code: {statusCode}");
                }

                // Extract the created project data and check it's not null
                var data = (result as Created<ProjectDTO>)?.Value;
                if (data == null)
                {
                    Assert.Fail("No project data was returned.");
                    return;
                }
                else
                {
                    // Save the returned project data for the next test
                    Projects.Add(data);
                }
            }

            
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Context?.Dispose();
        }
    }
}
