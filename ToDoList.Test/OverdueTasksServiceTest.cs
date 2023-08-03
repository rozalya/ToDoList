using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Core.Services;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Test
{
    public class OverdueTasksServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicatioDbRepository, ApplicatioDbRepository>()
                .AddSingleton<IOverdueTasksService, OverdueTasksService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void CloseTask()
        {
            var service = serviceProvider.GetService<IOverdueTasksService>();
            var result = service.CloseTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            Assert.That(result.IsCompletedSuccessfully, Is.True);
        }

        [Test]
        public void GetAllOverdueTask()
        {
            var service = serviceProvider.GetService<IOverdueTasksService>();
            var result = service.GetAllOverdueTasks("12345").TaskViewModel;
            Assert.That(result.Count == 3);
        }

        [Test]
        public void ChangeDateTask()
        {
            var task =  new TaskViewModel
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392"),
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(1),
                IsImportant = true,
            };

            var service = serviceProvider.GetService<IOverdueTasksService>();
            var result = service.EditDate(task, "12345");
            Assert.That(result.IsCompletedSuccessfully, Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {

            ActiveTask testTask = new ActiveTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today,
                IsImportant = true,
            };

            ActiveTask importantTask = new ActiveTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(-2),
                IsImportant = true,
            };

            ActiveTask stepsTask = new ActiveTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(-3),
                IsImportant = true,
            };
            stepsTask.Steps.Add(new Step
            {
                StepId = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465394"),
                Title = "This is step",
                TaskFK = stepsTask.Id
            });

            ActiveTask statemetsTask = new ActiveTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465395"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(-4),
                IsImportant = true,
            };
            statemetsTask.Statements.Add(new Statement
            {
                StatementId = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465396"),
                If = "This is if step",
                Then = "This is then step",
                TaskFK = statemetsTask.Id
            });
            await repo.AddAsync(statemetsTask);
            await repo.AddAsync(stepsTask);
            await repo.AddAsync(testTask);
            await repo.AddAsync(importantTask);
            await repo.SaveChangesAsync();

        }
    }
}
