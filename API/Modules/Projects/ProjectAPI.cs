using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Modules;

namespace API
{
    /// <summary>
    /// Provides HTTP endpoint implementations for CRUD operations on <see cref="Project"/> entities.
    /// </summary>
    /// <remarks>
    /// All members are implemented as static asynchronous methods and require a <see cref="PlannerDb"/>
    /// instance for database access. Methods return <see cref="IResult"/> values representing HTTP responses.
    /// </remarks>
    public class ProjectAPI
    {
        /// <summary>
        /// Retrieves all projects from the database.
        /// </summary>
        /// <param name="db">The <see cref="PlannerDb"/> database context used to query projects.</param>
        /// <returns>
        /// An <see cref="IResult"/> containing an HTTP 200 (OK) response with the list of <see cref="Project"/> entities.
        /// </returns>
        public static async Task<IResult> GetAllProjects(PlannerDb db)
        {
            return TypedResults.Ok(await db.Project.ToListAsync() ?? new List<Project>());
        }

        /// <summary>
        /// Retrieves a single project by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the project to retrieve.</param>
        /// <param name="db">The <see cref="PlannerDb"/> database context used to query the project.</param>
        /// <returns>
        /// An <see cref="IResult"/> that is:
        /// - HTTP 200 (OK) with the <see cref="Project"/> when found;
        /// - HTTP 404 (NotFound) when no project with the specified id exists.
        /// </returns>
        public static async Task<IResult> GetProjectById(int id, PlannerDb db)
        {
            return await db.Project.FindAsync(id)
                is Project project
                    ? TypedResults.Ok(project)
                    : TypedResults.NotFound();
        }

        /// <summary>
        /// Creates a new project from the provided <see cref="ProjectDTO"/> and persists it to the database.
        /// </summary>
        /// <param name="projectDTO">The data transfer object containing project properties to create.</param>
        /// <param name="db">The <see cref="PlannerDb"/> database context used to add the new project.</param>
        /// <returns>
        /// An <see cref="IResult"/> that is HTTP 201 (Created) with the created <see cref="ProjectDTO"/>.
        /// The Location header references the created resource.
        /// </returns>
        public static async Task<IResult> CreateProject(ProjectDTO projectDTO, PlannerDb db)
        {
            var project = new Project
            {
                Id = projectDTO.Id,
                Name = projectDTO.Name,
                Description = projectDTO.Description,
            };

            db.Project.Add(project);
            await db.SaveChangesAsync();

            projectDTO = new ProjectDTO(project);

            return TypedResults.Created($"/Taskitems/{project.Id}", projectDTO);
        }

        /// <summary>
        /// Updates an existing project identified by <paramref name="id"/> with values from <paramref name="inputProject"/>.
        /// </summary>
        /// <param name="id">The identifier of the project to update.</param>
        /// <param name="inputProject">The <see cref="Project"/> containing updated values.</param>
        /// <param name="db">The <see cref="PlannerDb"/> database context used to find and save the project.</param>
        /// <returns>
        /// An <see cref="IResult"/> that is:
        /// - HTTP 204 (NoContent) when the update succeeds;
        /// - HTTP 404 (NotFound) when no project with the specified id exists.
        /// </returns>
        public static async Task<IResult> UpdateProject(int id, Project inputProject, PlannerDb db)
        {
            var project = await db.Project.FindAsync(id);
            if (project is null) return TypedResults.NotFound();
            
            project.Name = inputProject.Name;
            project.Description = inputProject.Description;

            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        /// <summary>
        /// Deletes the project with the specified identifier from the database.
        /// </summary>
        /// <param name="id">The identifier of the project to delete.</param>
        /// <param name="db">The <see cref="PlannerDb"/> database context used to find and remove the project.</param>
        /// <returns>
        /// An <see cref="IResult"/> that is:
        /// - HTTP 204 (NoContent) when the deletion succeeds;
        /// - HTTP 404 (NotFound) when no project with the specified id exists.
        /// </returns>
        public static async Task<IResult> DeleteProject(int id, PlannerDb db)
        {
            if (await db.Project.FindAsync(id) is Project project)
            {
                db.Project.Remove(project);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        }
    }
}
