﻿using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Contracts;
using ToDoList.Core.Services;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicatioDbRepository, ApplicatioDbRepository>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IOverdueTasksService, OverdueTasksService>();
            services.AddScoped<IExpiredTasksService, ExpiredTasksService>();
            services.AddScoped<IDoneTasksService, DoneTasksService>();
            services.AddScoped<IStatementService, StatementService> ();
            services.AddScoped<IAdministrationService, AdministrationService> ();

            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
