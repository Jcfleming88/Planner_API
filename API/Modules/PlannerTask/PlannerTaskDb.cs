using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Modules
{
    class PlannerTaskDb(DbContextOptions<PlannerTaskDb> options) : DbContext(options)
    {
        public DbSet<PlannerTask> PlannerTask => Set<PlannerTask>();
    }
}