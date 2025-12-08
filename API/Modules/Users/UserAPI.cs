using Microsoft.EntityFrameworkCore;
using Modules;

namespace API
{
    public class UserAPI
    {
        public static async Task<IResult> GetAllUsers(PlannerDb db)
        {
            return TypedResults.Ok(await db.User.ToListAsync());
        }

        public static async Task<IResult> GetUserById(int id, PlannerDb db)
        {
            return await db.User.FindAsync(id)
                is User user
                    ? TypedResults.Ok(user)
                    : TypedResults.NotFound();
        }

        public static async Task<IResult> CreateUser(User user, PlannerDb db)
        {
            db.User.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/users/{user.Id}", user);
        }

        public static async Task<IResult> DeleteUser(int id, PlannerDb db)
        {
            var user = await db.User.FindAsync(id);
            if (user is null)
            {
                return TypedResults.NotFound();
            }
            db.User.Remove(user);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        public static async Task<IResult> UpdateUser(int id, User inputUser, PlannerDb db)
        {
            var user = await db.User.FindAsync(id);
            if (user is null) return TypedResults.NotFound();
            user.Name = inputUser.Name;
            user.Email = inputUser.Email;
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
    }
}
