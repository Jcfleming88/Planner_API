using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http.HttpResults;
using NUnit.Framework;

using API;
using Modules;
using Microsoft.EntityFrameworkCore;

namespace APITests
{
    public partial class TasksTests
    {
        [Test]
        [Category("Tasks")]
        [Order(1)]
        public async Task CreateTask()
        {
            // Create a new PlannerTaskDTO object with test data
            var newTask = new PlannerTaskDTO(new PlannerTask(
                projectId: FirstProject.Id,
                name: "New Task",
                owner: "Test User"
            ));

            // Create a new task in the database
            var result = await PlannerTaskAPI.CreateTask(
                newTask,
                Context
            );

            // Check for a successful return with the created task data
            Assert.That(
                result,
                Is.InstanceOf<Created<PlannerTaskDTO>>(),
                "The task was not created correctly."
            );

            return;
        }
    }
}
