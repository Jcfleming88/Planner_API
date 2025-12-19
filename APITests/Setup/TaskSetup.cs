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
        private ProjectDTO? FirstProject { get; set; } = null;

        internal const string DatabaseName = "TasksDb";

        private readonly ProjectDTO NewProject = new ProjectDTO(new Project(
            name: "New Project",
            description: "This is a new project."
        ));

        [OneTimeSetUp]
        public void Setup()
        { 
            Context = InMemoryDbFactory.CreateAndSeedContext(DatabaseName);

            // Create a new project in the database
            var result = ProjectAPI.CreateProject(
                NewProject,
                Context
            ).GetAwaiter().GetResult();

            // Check the project was created successfully
            if (result == null) {
                Assert.Fail("Setup failed: CreateProject returned null");
            }
            else
            {
                // Check the project was setup successfully
                var statusCode = (result as Created<ProjectDTO>).StatusCode;
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
                FirstProject = data;
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Context?.Dispose();
        }
    }
}
