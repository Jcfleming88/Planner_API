using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Modules
{
    public class PlannerTaskDb(DbContextOptions<PlannerTaskDb> options) : DbContext(options)
    {
        public DbSet<PlannerTask> PlannerTask { get; set; } = null!;
    }
}