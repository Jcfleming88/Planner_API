using Microsoft.EntityFrameworkCore;

namespace Modules
{
    public class PlannerDb : DbContext
    {
        public PlannerDb(DbContextOptions<PlannerDb> options) : base(options)
        {
        }

        public DbSet<Project> Project { get; set; } = null!;
        public DbSet<PlannerTask> PlannerTask { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<ProjectUser> ProjectUser { get; set; } = null!;
        public DbSet<TaskAssignee> TaskAssignee { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectUsers)
                .WithOne(pu => pu.Project)
                .HasForeignKey(pu => pu.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.PlannerTasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlannerTask>()
                .HasMany(t => t.TaskAssignees)
                .WithOne(ta => ta.PlannerTask)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskAssignee>()
                .HasOne(ta => ta.PlannerTask)
                .WithMany(t => t.TaskAssignees)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskAssignee>()
                .HasOne(ta => ta.User)
                .WithMany(u => u.Assignments)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
