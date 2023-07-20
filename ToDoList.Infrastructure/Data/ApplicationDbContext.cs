using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Data.Migrations;
using ToDoList.Infrastructure.InitialSeed;

namespace ToDoList.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new InitialDataConfiguration<ActiveTask>(@"InitialSeed/categories.json"));

            modelBuilder.Entity<ActiveTask>()
                .HasMany(e => e.Steps)
                .WithOne(e => e.ActiveTask)
                .HasForeignKey(e => e.TaskFK)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ActiveTask> ActiveTasks { get; set; }
        public DbSet<DoneTask> DoneTasks { get; set; }
        public DbSet<ExpiderTask> ExpiderTasks { get; set; }
        public DbSet<Step> Steps { get; set; }
    }
}

