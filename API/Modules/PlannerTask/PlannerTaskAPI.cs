using Microsoft.EntityFrameworkCore;
using Modules;

namespace API
{
    public class PlannerTaskAPI
    {
        public static async Task<IResult> GetAllTasks(PlannerTaskDb db) 
        {
            return TypedResults.Ok(await db.PlannerTask.ToListAsync());
        }

        public static async Task<IResult> GetTasksByProjectId(int projectId, PlannerTaskDb db)
        {
            var tasks = await db.PlannerTask
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
            return TypedResults.Ok(tasks);
        }

        public static async Task<IResult> GetTaskById(int id, PlannerTaskDb db)
        {
            return await db.PlannerTask.FindAsync(id)
                is PlannerTask plannerTask
                    ? TypedResults.Ok(plannerTask)
                    : TypedResults.NotFound();
        }

        public static async Task<IResult> CreateTask(PlannerTaskDTO plannerTaskDTO, PlannerTaskDb db)
        {
            var plannerTask = new PlannerTask
            {
                Name = plannerTaskDTO.Name,
                
            };

            db.PlannerTask.Add(plannerTask);
            await db.SaveChangesAsync();

            plannerTaskDTO = new PlannerTaskDTO(plannerTask);

            return TypedResults.Created($"/Taskitems/{plannerTaskDTO.Id}", plannerTaskDTO);
        }

        public static async Task<IResult> UpdateTask(int id, PlannerTask inputTask, PlannerTaskDb db)
        {
            var task = await db.PlannerTask.FindAsync(id);
            if (task is null) return TypedResults.NotFound();
            task.Name = inputTask.Name;
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        public static async Task<IResult> DeleteTask(int id, PlannerTaskDb db)
        {
            if (await db.PlannerTask.FindAsync(id) is PlannerTask plannerTask)
            {
                db.PlannerTask.Remove(plannerTask);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        }
    }
}
