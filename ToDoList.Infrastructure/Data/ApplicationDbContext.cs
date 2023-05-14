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
              modelBuilder.Entity<NewTask>()
                 .HasIndex(c => c.Id)
                .IsUnique();

            modelBuilder.ApplyConfiguration(new InitialDataConfiguration<NewTask>(@"InitialSeed/categories.json"));

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<NewTask> NewTasks { get; set; }
    }
}

