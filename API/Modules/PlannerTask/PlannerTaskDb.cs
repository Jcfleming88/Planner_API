using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Modules
{
    public class PlannerTaskDb : DbContext
    {
        public PlannerTaskDb(DbContextOptions<PlannerTaskDb> options) : base(options)
        {
        }

        public DbSet<PlannerTask> PlannerTask { get; set; } = null!;
    }
}