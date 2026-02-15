using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Modules;
using NUnit.Framework;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace APITests
{
    public partial class TaskAssignees
    {
        [Test, Category("TaskAssignees"), Order(1)]
        public async Task AddTaskAssignee_CreatesAssigneeAndReturnsCreated()
        {
            var user = Context.User.First();
            var task = Context.PlannerTask.First();
            var dto = new TaskAssigneeDTO ( new TaskAssignee(taskId: task.Id, userId: user.Id.ToString()));

            var result = await TaskAssigneeAPI.AddTaskAssignee(task.Id, dto, Context);

            Assert.That(result, Is.InstanceOf<Created<TaskAssigneeDTO>>());
            var created = (result as Created<TaskAssigneeDTO>)!;
            Assert.That(created.Value.TaskId, Is.EqualTo(task.Id));
            Assert.That(created.Value.UserId, Is.EqualTo(user.Id.ToString()));
        }

        [Test, Category("TaskAssignees"), Order(2)]
        public async Task GetTaskAssignees_ReturnsAssignees()
        {
            var task = Context.PlannerTask.First();
            var result = await TaskAssigneeAPI.GetTaskAssignees(task.Id, Context);
            Assert.That(result, Is.InstanceOf<Ok<List<TaskAssignee>>>());
            var list = (result as Ok<List<TaskAssignee>>)?.Value;
            Assert.That(list, Is.Not.Null);
            Assert.That(list!.Any(), Is.True);
        }

        [Test, Category("TaskAssignees"), Order(3)]
        public async Task AddTaskAssignee_ReturnsBadRequest_WhenTaskIdMismatch()
        {
            var user = Context.User.First();
            var task = Context.PlannerTask.First();
            var dto = new TaskAssigneeDTO(new TaskAssignee(taskId: task.Id + 1, userId: user.Id.ToString())); ;
            var result = await TaskAssigneeAPI.AddTaskAssignee(task.Id, dto, Context);
            Assert.That(result, Is.InstanceOf<BadRequest<string>>());
        }

        [Test, Category("TaskAssignees"), Order(4)]
        public async Task DeleteTaskAssignee_DeletesAssigneeAndReturnsNoContent()
        {
            var task = Context.PlannerTask.First();
            var user = Context.User.First();
            var check = await TaskAssigneeAPI.GetTaskAssignees(task.Id, Context);

            if (check is not Ok<List<TaskAssignee>> ok)
            {
                // Add an assignee to delete
                var assignee = new TaskAssignee(task.Id, user.Id);
                Context.TaskAssignee.Add(assignee);
                Context.SaveChanges();
            }

            var result = await TaskAssigneeAPI.DeleteTaskAssignee(task.Id, user.Id.ToString(), Context);
            Assert.That(result, Is.InstanceOf<NoContent>());
            // Ensure it's deleted
            var inDb = await Context.TaskAssignee.FirstOrDefaultAsync(a => a.TaskId == task.Id && a.UserId == user.Id.ToString());
            Assert.That(inDb, Is.Null);
        }

        [Test, Category("TaskAssignees"), Order(5)]
        public async Task DeleteTaskAssignee_ReturnsNotFound_WhenNotExists()
        {
            var result = await TaskAssigneeAPI.DeleteTaskAssignee(-999, "ghost", Context);
            Assert.That(result, Is.InstanceOf<NotFound>());
        }
    }
}
