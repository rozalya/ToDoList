using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Core.Services;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Test
{
    public class ExpiredTasksServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IApplicatioDbRepository repo;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicatioDbRepository, ApplicatioDbRepository>()
                .AddSingleton<IExpiredTasksService, ExpiredTasksService>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void GetAllExpiredTasks()
        {
            var service = serviceProvider.GetService<IExpiredTasksService>();
            var result = service.GetAllExpiredTasks("12345").TaskViewModel;
            Assert.That(result.Count == 3);
        }

        [Test]
        public void GetExpiredTaskDetails()
        {
            var service = serviceProvider.GetService<IExpiredTasksService>();
            var result = repo.All<ExpiderTask>().Where(x => x.Id == Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392")).First();
            var expTaskDueDate = result.DueDate;
            var expTaskId = result.Id;
            var expTaskIsIprt = result.IsImportant;
            var expTaskNote = result.Note;
            var expTaskUserId = result.UserId;
            Assert.That(expTaskDueDate, Is.EqualTo(DateTime.Today.AddDays(-2)));
            Assert.That(expTaskId, Is.EqualTo(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392")));
            Assert.That(expTaskIsIprt, Is.True);
            Assert.That(expTaskNote, Is.EqualTo("Some text to test here"));
            Assert.That(expTaskUserId, Is.EqualTo("12345"));
        }

        [Test]
        public void DeleteExpiredTasks()
        {
            var service = serviceProvider.GetService<IExpiredTasksService>();
            var result = service.DeleteTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392"));
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

            ExpiderTask importantTask = new ExpiderTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(-2),
                IsImportant = true,
            };

            ExpiderTask stepsTask = new ExpiderTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(-3),
                IsImportant = true,
            };

            ExpiderTask statemetsTask = new ExpiderTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465395"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(-4),
                IsImportant = true,
            };
            await repo.AddAsync(statemetsTask);
            await repo.AddAsync(stepsTask);
            await repo.AddAsync(testTask);
            await repo.AddAsync(importantTask);
            await repo.SaveChangesAsync();

        }
    }
}
