using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Core.Contracts;
using ToDoList.Core.Services;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Test
{
    public class TasksServiceTest
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
                .AddSingleton<ITasksService, TasksService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void GetTodayTask()
        {
            var service = serviceProvider.GetService<ITasksService>();
            var searchedTasks = service.GetTodayTasks("12345");
            Assert.That(searchedTasks.TaskViewModel.Count == 1);
            Assert.That(searchedTasks.TaskViewModel[0].DueDate, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void GetTodayTaskEmpty()
        {
            var service = serviceProvider.GetService<ITasksService>();
            var searchedTasks = service.GetTodayTasks("12367");
            Assert.That(searchedTasks.TaskViewModel.Count == 0);
        }

        [Test]
        public void GetPlannedTask()
        {
            var service = serviceProvider.GetService<ITasksService>();
            var searchedTasks = service.GetPlannedTasks("12345");
            Assert.That(searchedTasks.TaskViewModel.Count == 3);
        }

        [Test]
        public void GetImportantTask()
        {
            var service = serviceProvider.GetService<ITasksService>();
            var searchedTasks = service.GetImportantTasks("12345");
            Assert.That(searchedTasks.TaskViewModel.Count == 3);
        }

        [Test]
        public void GetStepsAddTask()
        {
            var service = serviceProvider.GetService<ITasksService>();
            var searchedTasks = service.GetTaskWithSteps("12345").TaskViewModel;
            Assert.That(searchedTasks.Count == 1);
            Assert.That(searchedTasks.First().Steps.First().Title, Is.EqualTo("This is step"));
        }

        [Test]
        public void GetStatementsAddTask()
        {
            var service = serviceProvider.GetService<ITasksService>();
            var searchedTasks = service.GetTaskWithStatements("12345").TaskViewModel;
            Assert.That(searchedTasks.Count == 1);
            Assert.That(searchedTasks.First().Statements.First().If, Is.EqualTo("This is if step"));
            Assert.That(searchedTasks.First().Statements.First().Then, Is.EqualTo("This is then step"));
        }

        [Test]
        public void GetAllActiveTask()
        {
            var service = serviceProvider.GetService<ITasksService>();
            var searchedTasks = service.GetAllTasks("12345");
            Assert.That(searchedTasks.TaskViewModel.Count == 4);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            ActiveTask todaysTask = new ActiveTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today,
                IsImportant = false,
            };

            ActiveTask importantTask = new ActiveTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392"),
                UserId = "12345",
                Note = "Some text to test here",
                IsImportant = true,
            };

            ActiveTask stepsTask = new ActiveTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(2),
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
                DueDate= DateTime.Today.AddDays(1),
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
            await repo.AddAsync(todaysTask);
            await repo.AddAsync(importantTask);
            await repo.SaveChangesAsync();
        }
    }
}
