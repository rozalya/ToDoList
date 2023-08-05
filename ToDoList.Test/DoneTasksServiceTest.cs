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
    public class DoneTasksServiceTest
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
                .AddSingleton<IDoneTasksService, DoneTasksService>()
                 .AddSingleton<ITaskService, TaskService>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void GetAllDoneTasks()
        {
            var service = serviceProvider.GetService<IDoneTasksService>();
            var result = service.GetAllDoneTasks("12345");
            Assert.That(result.DoneTaskListViewModels.Count == 3);
        }

        [Test]
        public void ReopenDoneTasks()
        {
            var serviceDoneTask = serviceProvider.GetService<IDoneTasksService>();
            var serviceActivTask = serviceProvider.GetService<ITaskService>();
            var result = serviceDoneTask.ReopenTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392"));
            var task = serviceActivTask.GetTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392"));
            
            Assert.That(result.IsCompletedSuccessfully, Is.True);
            Assert.That(task.Result.IsColsed, Is.False);
        }

        [Test]
        public void GetDoneTask()
        {
            var service = serviceProvider.GetService<IDoneTasksService>();
            var result = service.GetTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393"));

            Assert.That(result.IsCompletedSuccessfully, Is.True);
            Assert.That(result.Result.Id == Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393"));
        }

        [Test]
        public void RateDoneTasks()
        {
            var service = serviceProvider.GetService<IDoneTasksService>();
            var rate = new RateTaskViewModel
            {
                FirstStar = 3,
                SecondStar = 4,
                ThirdStar = 5,
            };

            var result = service.AddRate(rate, Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393"));
            var ratedTask = service.GetTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393")).Result;
            var rates = repo.All<Rate>().ToList();
            var rateTasks = repo.All<DoneTask>().Where(x => x.Id == Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393"));
            var rateTask = rates.First().DoneTask;
            var rateId = rates.First().RateId;
            var rateTaskFk = rates.First().TaskFK;


            Assert.That(result.IsCompletedSuccessfully, Is.True);
            Assert.That(ratedTask.Rate.FirstStar, Is.EqualTo(3));
            Assert.That(ratedTask.Rate.SecondStar, Is.EqualTo(4));
            Assert.That(ratedTask.Rate.ThirdStar, Is.EqualTo(5));
            Assert.That(rateTask.Id, Is.EqualTo(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393")));
            Assert.That(rateTaskFk, Is.EqualTo(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393")));
            Assert.That(rateId, Is.Not.Null);
            Assert.That(rateTasks.First().Rate, Is.Not.Null);
        }

        [Test]
        public void GetAppDB()
        {
            var result = repo.All<ActiveTask>();
            var result2 = repo.All<DoneTask>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result2, Is.Not.Null);
        }

        /*[Test]
        public void DeleteAppDB()
        {
            var result = repo.All<ActiveTask>().Where(x => x.Id == Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391")).First();
            repo.Delete(result);
            repo.SaveChanges();

            var result3 = repo.All<ActiveTask>().Where(x => x.Id == Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391")).FirstOrDefault();

            Assert.That(result, Is.Not.Null);
            Assert.That(result3, Is.Null);
        }*/

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

            DoneTask importantTask = new DoneTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465392"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(1),
                IsImportant = true,
            };

            DoneTask stepsTask = new DoneTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465393"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(2),
                IsImportant = true,
            };

            DoneTask statemetsTask = new DoneTask
            {
                Id = Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465395"),
                UserId = "12345",
                Note = "Some text to test here",
                DueDate = DateTime.Today.AddDays(3),
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
