using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Contracts;
using ToDoList.Core.Services;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Api.Extensions
{
    public static class ApiServiceCollectionExtension
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicatioDbRepository, ApplicatioDbRepository>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }

        public static IServiceCollection AddApiDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
