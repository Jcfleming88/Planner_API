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
        [Test, Category("Tasks"), Order(1)]
        public async Task CreateTask()
        {
            // Create new PlannerTaskDTO object with test data
            var tasks = new List<PlannerTask>() 
            {
                new PlannerTask(
                    projectId: Projects[0].Id,
                    name: "Task 1",
                    owner: User1
                ),
                new PlannerTask(
                    projectId: Projects[0].Id,
                    name: "Task 2",
                    owner: User2
                ),
                new PlannerTask(
                    projectId: Projects[1].Id,
                    name: "Task 3",
                    owner: User1
                )
            };

            foreach (var task in tasks) 
            {
                // Create a new task in the database
                var result = await PlannerTaskAPI.CreateTask(
                    task,
                    Context
                );

                // Check for a successful return with the created task data
                Assert.That(
                    result,
                    Is.InstanceOf<Created<PlannerTaskDTO>>(),
                    $"The task '{task.Name}' was not created correctly."
                );

                var createdTask = (result as Created<PlannerTaskDTO>)!.Value;

                // Check the created task is not null and that a value was returned
                if (createdTask == null)
                {
                    Assert.Fail("A null task was returned by the API.");
                }

                // Check the name of the created task
                Assert.That(
                    createdTask!.Name,
                    Is.EqualTo(task.Name),
                    "The created task does not have the correct name."
                );
                Tasks.Add(createdTask!);
            }

            return;
        }

        //[Test, Category("Tasks"), Order(2)]
        //public async Task GetAllTask()
        //{
        //    // Get a list of all tasks from the database
        //    var result = await PlannerTaskAPI.GetAllTasks(Context);

        //    // Check for a successful return with a list of tasks
        //    Assert.That(
        //        result,
        //        Is.InstanceOf<Ok<List<PlannerTask>>>(),
        //        "The result should be an HTTP 200 Ok response."
        //    );

        //    // Check that the list of tasks is not null
        //    var data = (result as Ok<List<PlannerTask>>)?.Value;
        //    Assert.That(
        //        data,
        //        Is.Not.Null,
        //        "The returned list of tasks should not be null."
        //    );

        //    // Check that at least one task exists in the database
        //    Assert.That(
        //        data!.Count,
        //        Is.GreaterThanOrEqualTo(3),
        //        "There should be at least 3 tasks in the database."
        //    );
        //    return;
        //}

        [Test, Category("Tasks"), Order(3)]
        public async Task GetProjectTasks()
        {
            // Get tasks for the first project
            var result = await PlannerTaskAPI.GetTasksByProjectId(
                Projects[0].Id,
                Context
            );

            // Check for a successful return with a list of tasks
            Assert.That(
                result,
                Is.InstanceOf<Ok<List<PlannerTask>>>(),
                "The result should be an HTTP 200 Ok response."
            );

            // Check that the list of tasks is not null
            var data = (result as Ok<List<PlannerTask>>)?.Value;
            Assert.That(
                data,
                Is.Not.Null,
                "The returned list of tasks should not be null."
            );

            // Check that the correct number of tasks are returned for the project
            Assert.That(
                data!.Count,
                Is.EqualTo(2),
                "There should be exactly 2 tasks for the first project."
            );

            return;
        }
    }
}
