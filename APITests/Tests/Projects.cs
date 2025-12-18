using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http.HttpResults;
using NUnit.Framework;

using API;
using Modules;
using Microsoft.EntityFrameworkCore;

namespace APITests
{
    public partial class Tests
    {
        [Test]
        [Category("Projects")]
        public async Task GetAllProjects()
        {
            var result = await ProjectAPI.GetAllProjects(Context);

            Assert.That(
                result,
                Is.InstanceOf<Ok<List<Project>>>(),
                "The result should be an HTTP 200 Ok response."
                );

            return;
        }

        [Test]
        [Category("Projects")]
        public async Task CreateProject()
        {
            var newProject = new ProjectDTO(new Project(
                id: 10,
                name: "New Project",
                description: "This is a new project."
            ));

            var result = await ProjectAPI.CreateProject(
                newProject, 
                Context
            );

            var data = (result as Created<ProjectDTO>)?.Value;

            Assert.That(
                result,
                Is.InstanceOf<Created<ProjectDTO>>(),
                "The project was not created correctly."
                );

            if ( data == null)
            {
                Assert.Fail("No project data was returned.");
                return;
            }

            // Check the database to ensure the project was added
            var retrievedProject = await ProjectAPI.GetProjectById(data.Id, Context);

            Assert.That(
                retrievedProject,
                Is.InstanceOf<Ok<Project>>(),
                "The new project was not found in the database."
                );

            if ((retrievedProject as Ok<Project>)?.Value == null)
            {
                Assert.Fail("No project data was retrieved via the GetProjectyId function");
            } 
            else
            {
                Assert.That(
                    (retrievedProject as Ok<Project>)?.Value.Name == newProject.Name,
                    "The new project's name does not match."
                    );
            }

            return;
        }
    }
}
