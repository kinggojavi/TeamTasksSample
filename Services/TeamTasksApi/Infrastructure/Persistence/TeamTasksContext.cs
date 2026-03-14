using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Domain.Entities;
using TeamTasksApi.Domain.Views;

namespace TeamTasksApi.Infrastructure.Persistence
{
    public class TeamTasksContext : DbContext
    {
        public TeamTasksContext(DbContextOptions<TeamTasksContext> options) : base(options) { }

        //Tablas
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }


        // Vistas
        public DbSet<DeveloperLoadSummaryView> DeveloperLoadSummary { get; set; }
        public DbSet<ProjectStatusSummaryView> ProjectStatusSummary { get; set; }
        public DbSet<TasksDueSoonView> TasksDueSoon { get; set; }
        public DbSet<DeveloperRiskSummaryView> DeveloperRiskSummary { get; set; }

        // Procedimiento almacenado
        public DbSet<TaskProcedureResult> TaskProcedureResults { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Developers
            modelBuilder.Entity<Developer>(entity =>
            {
                entity.ToTable("Developers", schema: "TeamTasks");
                entity.HasKey(d => d.DeveloperId);
            });

            // Projects
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects", schema: "TeamTasks");
                entity.HasKey(p => p.ProjectId);

                // Guardar enum ProjectStatus como string
                entity.Property(p => p.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);
            });

            // Tasks
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("Tasks", schema: "TeamTasks");
                entity.HasKey(t => t.TaskId);

                // Guardar enum TaskStatus como string
                entity.Property(t => t.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                // Guardar enum TaskPriority como string
                entity.Property(t => t.Priority)
                      .HasConversion<string>()
                      .HasMaxLength(10);

                // Relaciones
                entity.HasOne(t => t.Project)
                      .WithMany(p => p.Tasks)
                      .HasForeignKey(t => t.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Assignee)
                      .WithMany(d => d.Tasks)
                      .HasForeignKey(t => t.AssigneeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DeveloperLoadSummaryView>().HasNoKey().ToView("vw_DeveloperLoadSummary");
            modelBuilder.Entity<ProjectStatusSummaryView>().HasNoKey().ToView("vw_ProjectStatusSummary");
            modelBuilder.Entity<TasksDueSoonView>().HasNoKey().ToView("vw_TasksDueSoon");
            modelBuilder.Entity<DeveloperRiskSummaryView>().HasNoKey().ToView("vw_DeveloperRiskSummary");

            modelBuilder.Entity<TaskProcedureResult>().HasNoKey();
        }
    }
}
