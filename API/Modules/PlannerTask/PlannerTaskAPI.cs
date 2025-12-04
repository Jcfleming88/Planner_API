using Microsoft.EntityFrameworkCore;
using Modules;

namespace API
{
    /// <summary>
    /// Provides HTTP API handlers for CRUD operations on planner tasks.
    /// </summary>
    /// <remarks>
    /// Methods on this static class are intended to be used as minimal API endpoints.
    /// Each method interacts with the provided <c>PlannerTaskDb</c> to perform database operations
    /// and returns an <c>IResult</c> representing the appropriate HTTP response.
    /// </remarks>
    public class PlannerTaskAPI
    {
        /// <summary>
        /// Retrieves all planner tasks from the database.
        /// </summary>
        /// <param name="db">The database context used to access planner tasks (injected).</param>
        /// <returns>
        /// An <see cref="IResult"/> containing HTTP 200 (OK) with a collection of all planner tasks.
        /// </returns>
        public static async Task<IResult> GetAllTasks(PlannerTaskDb db) 
        {
            return TypedResults.Ok(await db.PlannerTask.ToListAsync());
        }

        /// <summary>
        /// Retrieves planner tasks that belong to the specified project.
        /// </summary>
        /// <param name="projectId">The identifier of the project whose tasks should be returned.</param>
        /// <param name="db">The database context used to query planner tasks (injected).</param>
        /// <returns>
        /// An <see cref="IResult"/> containing HTTP 200 (OK) with a collection of tasks for the given project.
        /// If no tasks exist for the project, an empty collection is returned.
        /// </returns>
        public static async Task<IResult> GetTasksByProjectId(int projectId, PlannerTaskDb db)
        {
            var tasks = await db.PlannerTask
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
            return TypedResults.Ok(tasks);
        }

        /// <summary>
        /// Retrieves a single planner task by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the planner task to retrieve.</param>
        /// <param name="db">The database context used to find the planner task (injected).</param>
        /// <returns>
        /// An <see cref="IResult"/> that is HTTP 200 (OK) with the <c>PlannerTask</c> if found,
        /// or HTTP 404 (NotFound) if no task with the specified id exists.
        /// </returns>
        public static async Task<IResult> GetTaskById(int id, PlannerTaskDb db)
        {
            return await db.PlannerTask.FindAsync(id)
                is PlannerTask plannerTask
                    ? TypedResults.Ok(plannerTask)
                    : TypedResults.NotFound();
        }

        /// <summary>
        /// Creates a new planner task from the provided DTO and persists it to the database.
        /// </summary>
        /// <param name="plannerTaskDTO">The DTO containing data for the new planner task.</param>
        /// <param name="db">The database context used to add and save the new task (injected).</param>
        /// <returns>
        /// An <see cref="IResult"/> that is HTTP 201 (Created) with the created <c>PlannerTaskDTO</c>
        /// and a Location header pointing to the created resource.
        /// </returns>
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

        /// <summary>
        /// Updates an existing planner task with the provided input.
        /// </summary>
        /// <param name="id">The identifier of the task to update.</param>
        /// <param name="inputTask">The task data to apply to the existing task.</param>
        /// <param name="db">The database context used to find and save the task (injected).</param>
        /// <returns>
        /// An <see cref="IResult"/> that is HTTP 204 (NoContent) when the update succeeds,
        /// or HTTP 404 (NotFound) if the task does not exist.
        /// </returns>
        public static async Task<IResult> UpdateTask(int id, PlannerTask inputTask, PlannerTaskDb db)
        {
            var task = await db.PlannerTask.FindAsync(id);
            if (task is null) return TypedResults.NotFound();
            task.Name = inputTask.Name;
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        /// <summary>
        /// Deletes a planner task by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the planner task to delete.</param>
        /// <param name="db">The database context used to remove and save changes (injected).</param>
        /// <returns>
        /// An <see cref="IResult"/> that is HTTP 204 (NoContent) when deletion succeeds,
        /// or HTTP 404 (NotFound) if no task with the specified id exists.
        /// </returns>
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
