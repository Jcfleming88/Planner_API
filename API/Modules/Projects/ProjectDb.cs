using Microsoft.EntityFrameworkCore;

namespace Modules
{
    public class ProjectDb(DbContextOptions<ProjectDb> options) : DbContext(options)
    {
        public DbSet<Project> Project { get; set; } = null!;
    }
}
