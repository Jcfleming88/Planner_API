using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using API; // Assuming ProjectUserAPI is in the API namespace
using Modules; // Assuming ProjectUser and PlannerDb are in the Modules namespace

namespace APITests
{
    public partial class ProjectUserTests
    {
        //[Test, Category("ProjectUser"), Order(1)]
        //public async Task CreateProjectUser()
        //{
        //    // Arrange
        //    var newProjectUser = new ProjectUser("testuser1", 1, 1);

        //    // Act
        //    var result = await ProjectUserAPI.CreateProjectUser(Context, newProjectUser);

        //    // Assert
        //    Assert.That(result, Is.InstanceOf<Created<ProjectUser>>(), "The project user was not created correctly.");

        //    var data = (result as Created<ProjectUser>)?.Value;
        //    Assert.That(data, Is.Not.Null, "No project user data was returned.");
        //    Assert.That(data!.ProjectId, Is.EqualTo(newProjectUser.ProjectId), "Project ID does not match.");
        //    Assert.That(data.UserId, Is.EqualTo(newProjectUser.UserId), "User ID does not match.");
        //    Assert.That(data.Role, Is.EqualTo(newProjectUser.Role), "Role does not match.");
        //    Assert.That((result as Created<ProjectUser>)!.Location, Is.EqualTo($"/projectusers/{newProjectUser.ProjectId}/{newProjectUser.UserId}"), "Location header is incorrect.");

        //    // Verify in DB
        //    var projectUserInDb = await Context.ProjectUser.FirstOrDefaultAsync(pu => pu.ProjectId == newProjectUser.ProjectId && pu.UserId == newProjectUser.UserId);
        //    Assert.That(projectUserInDb, Is.Not.Null, "Project user was not found in the database after creation.");
        //    Assert.That(projectUserInDb!.Role, Is.EqualTo(newProjectUser.Role), "Role in DB does not match.");

        //    FirstProjectUser = data; // Save for subsequent ordered tests
        //}

        [Test, Category("ProjectUser"), Order(2)]
        public async Task GetProjectUsers_ReturnsMultipleUsers()
        {
            // Arrange
            var projectIdForTest = 10;
            Context.ProjectUser.AddRange(
                new ProjectUser ( projectId: projectIdForTest, userId: "userA", role: 1 ),
                new ProjectUser ( projectId: projectIdForTest, userId: "userB", role: 2 ),
                new ProjectUser ( projectId: 99, userId: "otheruser", role: 1 ) // Different project
            );
            await Context.SaveChangesAsync();

            // Act
            var result = await ProjectUserAPI.GetProjectUsers(projectIdForTest, Context);

            // Assert
            Assert.That(result, Is.InstanceOf<Ok<List<ProjectUser>>>(), "The result should be an HTTP 200 Ok response.");

            var data = (result as Ok<List<ProjectUser>>)?.Value;
            Assert.That(data, Is.Not.Null, "No list of project users was returned.");
            Assert.That(data!.Count, Is.EqualTo(2), "Incorrect number of project users returned.");
            Assert.That(data.Any(pu => pu.UserId == "userA" && pu.ProjectId == projectIdForTest), Is.True, "UserA not found for the correct project.");
            Assert.That(data.Any(pu => pu.UserId == "userB" && pu.ProjectId == projectIdForTest), Is.True, "UserB not found for the correct project.");
            Assert.That(data.Any(pu => pu.UserId == "otheruser"), Is.False, "User from different project included.");
        }

        [Test, Category("ProjectUser"), Order(3)]
        public async Task GetProjectUsers_ReturnsEmptyListForNoUsers()
        {
            // Arrange
            var projectIdForTest = 20; // No users for this project

            // Act
            var result = await ProjectUserAPI.GetProjectUsers(projectIdForTest, Context);

            // Assert
            Assert.That(result, Is.InstanceOf<Ok<List<ProjectUser>>>(), "The result should be an HTTP 200 Ok response.");

            var data = (result as Ok<List<ProjectUser>>)?.Value;
            Assert.That(data, Is.Not.Null, "No list of project users was returned.");
            Assert.That(data!.Count, Is.EqualTo(0), "Expected an empty list of project users.");
        }

        [Test, Category("ProjectUser"), Order(4)]
        public async Task UpdateProjectUserRole_UpdatesRoleAndReturnsNoContent()
        {
            // Arrange
            var projectId = 2;
            var userId = "jane_doe";
            var initialRole = 1;
            var newRole = 3;

            Context.ProjectUser.Add(new ProjectUser ( projectId: projectId, userId: userId, role: initialRole ));
            await Context.SaveChangesAsync();

            // Act
            var result = await ProjectUserAPI.UpdateProjectUserRole(projectId, userId, newRole, Context);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContent>(), "The project user role was not updated correctly.");

            // Verify in DB
            var updatedProjectUser = await Context.ProjectUser.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);
            Assert.That(updatedProjectUser, Is.Not.Null, "Project user was not found after update.");
            Assert.That(updatedProjectUser!.Role, Is.EqualTo(newRole), "Project user role was not updated in the database.");
        }

        [Test, Category("ProjectUser"), Order(5)]
        public async Task UpdateProjectUserRole_ReturnsNotFoundWhenUserDoesNotExist()
        {
            // Arrange
            var projectId = 3;
            var userId = "nonexistent_user";
            var newRole = 2;

            // Act
            var result = await ProjectUserAPI.UpdateProjectUserRole(projectId, userId, newRole, Context);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFound>(), "Expected NotFound result when user does not exist.");
        }

        [Test, Category("ProjectUser"), Order(6)]
        public async Task DeleteProjectUser_RemovesUserAndReturnsNoContent()
        {
            // Arrange
            var projectId = 4;
            var userId = "john_doe";
            Context.ProjectUser.Add(new ProjectUser ( projectId: projectId, userId: userId, role: 1 ));
            await Context.SaveChangesAsync();

            // Act
            var result = await ProjectUserAPI.DeleteProjectUser(projectId, userId, Context);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContent>(), "The project user was not deleted correctly.");

            // Verify in DB
            var deletedProjectUser = await Context.ProjectUser.FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);
            Assert.That(deletedProjectUser, Is.Null, "Project user was still found in the database after deletion.");
        }

        [Test, Category("ProjectUser"), Order(7)]
        public async Task DeleteProjectUser_ReturnsNotFoundWhenUserDoesNotExist()
        {
            // Arrange
            var projectId = 5;
            var userId = "ghost_user";

            // Act
            var result = await ProjectUserAPI.DeleteProjectUser(projectId, userId, Context);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFound>(), "Expected NotFound result when user does not exist for deletion.");
        }
    }
}