using Modules;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class ProjectUserAPI
    {
        public static async Task<IResult> GetProjectUsers(int projectId, PlannerDb db)
        {
            var projectUsers = await db.ProjectUser
                .Where(pu => pu.ProjectId == projectId)
                .ToListAsync();
            return TypedResults.Ok(projectUsers);
        }

        public static async Task<IResult> CreateProjectUser(PlannerDb db, ProjectUser user)
        {
            db.ProjectUser.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/projectusers/{user.ProjectId}/{user.UserId}", user);
        }

        public static async Task<IResult> UpdateProjectUserRole(int projectId, string userId, int newRole, PlannerDb db)
        {
            var projectUser = await db.ProjectUser
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);
            if (projectUser == null)
            {
                return TypedResults.NotFound();
            }
            projectUser.Role = newRole;
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        public static async Task<IResult> DeleteProjectUser(int projectId, string userId, PlannerDb db)
        {
            var projectUser = await db.ProjectUser
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);
            if (projectUser == null)
            {
                return TypedResults.NotFound();
            }
            db.ProjectUser.Remove(projectUser);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
    }
}
