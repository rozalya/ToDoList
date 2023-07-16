using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ActiveTask> ActiveTasks { get; set; }
        public DbSet<DoneTask> DoneTasks { get; set; }
        public DbSet<ExpiderTask> ExpiderTasks { get; set; }
    }
}

