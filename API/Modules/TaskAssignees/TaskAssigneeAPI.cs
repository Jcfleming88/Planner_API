using Modules;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class TaskAssigneeAPI
    {
        public static async Task<IResult> GetTaskAssignees(int taskId, PlannerDb db)
        {
            var assignees = await db.TaskAssignee
                .Where(ta => ta.TaskId == taskId)
                .ToListAsync();

            return Results.Ok(assignees);
        }

        public static async Task<IResult> AddTaskAssignee(int taskId, TaskAssigneeDTO taskAssigneeDTO, PlannerDb db)
        {
            if (taskId != taskAssigneeDTO.TaskId)
            {
                return Results.BadRequest("Task ID in URL does not match Task ID in body.");
            }
            var taskAssignee = new TaskAssignee(taskAssigneeDTO.TaskId, taskAssigneeDTO.UserId);
            db.TaskAssignee.Add(taskAssignee);
            await db.SaveChangesAsync();
            return Results.Created($"/taskassignees/{taskAssignee.TaskId}/{taskAssignee.UserId}", new TaskAssigneeDTO(taskAssignee));
        }

        public static async Task<IResult> DeleteTaskAssignee(int taskId, string userId, PlannerDb db)
        {
            var taskAssignee = await db.TaskAssignee
                .FirstOrDefaultAsync(ta => ta.TaskId == taskId && ta.UserId == userId);
            if (taskAssignee == null)
            {
                return Results.NotFound();
            }
            db.TaskAssignee.Remove(taskAssignee);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
