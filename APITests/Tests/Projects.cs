using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http.HttpResults;
using NUnit.Framework;

using API;
using Modules;
using Microsoft.EntityFrameworkCore;

namespace APITests
{
    public partial class ProjectsTests
    {
        [Test]
        [Category("Projects")]
        [Order(1)]
        public async Task CreateProject()
        {
            // Create a new project in the database
            var result = await ProjectAPI.CreateProject(
                NewProject,
                Context
            );

            // Check for a successful return with the created project data
            Assert.That(
                result,
                Is.InstanceOf<Created<ProjectDTO>>(),
                "The project was not created correctly."
                );

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

            return;
        }


        [Test]
        [Category("Projects")]
        [Order(2)]
        public async Task GetAllProjects()
        {
            // Get a list of all projects from the database
            var result = await ProjectAPI.GetAllProjects(Context);

            // Check for a successful return with a list of projects
            Assert.That(
                result,
                Is.InstanceOf<Ok<List<Project>>>(),
                "The result should be an HTTP 200 Ok response."
                );

            // Check that the list of projects is not null
            if ((result as Ok<List<Project>>)?.Value == null)
            {
                Assert.Fail("No list of projects were returned as part of this test.");
            }

            // Check that there is at least one project in the list (the one we just created)
            Assert.That(
                (result as Ok<List<Project>>)?.Value.Count == 1,
                "No pojects where found. There should be one project in the test database."
                );

            return;
        }

        [Test]
        [Category("Projects")]
        [Order(3)]
        public async Task GetProjectById()
        {
            // Check the first project is not null and skip the test if it was not saved in the earlier test
            if (FirstProject == null)
            {
                Assert.Ignore("First project is null. Run the full category of tests to check. Skipping test.");
                return;
            }

            // Call for the first project we created by it's Id
            var retrievedProject = await ProjectAPI.GetProjectById(FirstProject.Id, Context);

            // Check for a successful return with the project data
            Assert.That(
                retrievedProject,
                Is.InstanceOf<Ok<Project>>(),
                "The new project was not found in the database."
                );

            // Check that the retrieved project data is not null
            var data = (retrievedProject as Ok<Project>)?.Value;
            if (data == null)
            {
                Assert.Fail("No project data was retrieved via the GetProjectyId function");
            }
            else
            {
                Assert.That(
                    FirstProject.Name == data.Name,
                    "The new project's name does not match."
                );
            }

            return;
        }

        [Test]
        [Category("Projects")]
        [Order(4)]
        public async Task UpdateProject()
        {
            // Check the first project is not null and skip the test if it was not saved in the earlier test
            if (FirstProject == null)
            {
                Assert.Ignore("First project is null. Run the full category of tests to check. Skipping test.");
                return;
            }

            // Update the first project we created
            var update = new Project
            {
                Name = "Updated Project Name",
                Description = "This project has been updated."
            };
            var result = await ProjectAPI.UpdateProject(
                FirstProject.Id,
                update,
                Context
            );

            // Check for a successful return with the updated project data
            Assert.That(
                result,
                Is.InstanceOf<NoContent>(),
                "The project was not updated correctly."
                );

            // Call for the first project we created by it's Id
            var updatedProject = await ProjectAPI.GetProjectById(FirstProject.Id, Context);

            // Check for a successful return with the project data
            Assert.That(
                updatedProject,
                Is.InstanceOf<Ok<Project>>(),
                "The updated project was not found in the database."
                );

            // Extract the updated project data and check it's not null
            var data = (updatedProject as Ok<Project>)?.Value;
            if (data == null)
            {
                Assert.Fail("No project data was returned after the update.");
            }
            else
            {
                Assert.That(
                    data.Name == "Updated Project Name",
                    "The project's name was not updated correctly."
                );
            }
            return;
        }

        [Test]
        [Category("Projects")]
        [Order(5)]
        public async Task DeleteProject()
        {
            // Check the first project is not null and skip the test if it was not saved in the earlier test
            if (FirstProject == null)
            {
                Assert.Ignore("First project is null. Run the full category of tests to check. Skipping test.");
                return;
            }

            // Delete the first project we created
            var result = await ProjectAPI.DeleteProject(
                FirstProject.Id,
                Context
            );

            // Check for a successful return indicating deletion
            Assert.That(
                result,
                Is.InstanceOf<NoContent>(),
                "The project was not deleted correctly."
                );

            // Try to retrieve the deleted project by its Id
            var deletedProject = await ProjectAPI.GetProjectById(FirstProject.Id, Context);

            // Check that the project is not found
            Assert.That(
                deletedProject,
                Is.InstanceOf<NotFound>(),
                "The deleted project was still found in the database."
                );

            return;
        }
    }
}
