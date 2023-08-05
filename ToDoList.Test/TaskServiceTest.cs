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
    public class TaskServiceTest
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
                .AddSingleton<ITaskService, TaskService>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void CreateTask()
        {
            TaskViewModel newTask = new TaskViewModel
            {
                Note = "Some text to test here",
                DueDate = DateTime.Today,
                IsImportant = true,
            };
            var userId = "123456";

            var service = serviceProvider.GetService<ITaskService>();
            Assert.DoesNotThrowAsync(async () => await service.NewTask(newTask, userId));
        }

        [Test]
        public void EditTask()
        {
            var service = serviceProvider.GetService<ITaskService>();
            var task = service.GetTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            task.Result.Note = "New text";
            service.EditTask(task.Result, "12345");
            var editedTask =  service.GetTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            Assert.That(editedTask.Result.Note, Is.EqualTo("New text"));
        }

        [Test]
        public void GetSomeTask()
        {
            var service = serviceProvider.GetService<ITaskService>();
            var searchedTask = service.GetTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            Assert.That(searchedTask.Result.Id, Is.EqualTo(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391")));
        }
        [Test]
        public void GetNullTask()
        {
            var service = serviceProvider.GetService<ITaskService>();
            var result = service.GetTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465555"));
            Assert.That(result.Result, Is.Null);           
        }

        [Test]
        public void DeleteSomeTask()
        {
            var service = serviceProvider.GetService<ITaskService>();
            var result = service.DeleteTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            Assert.That(result.IsCompletedSuccessfully, Is.True);
        }

        [Test]
        public void CompleteSomeTask()
        {
            var service = serviceProvider.GetService<ITaskService>();
            var result = service.CompleteTask(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            Assert.That(result.IsCompletedSuccessfully, Is.True);
        }

        [Test]
        public void AddStepToTask()
        {
            var service = serviceProvider.GetService<ITaskService>();
            var result = service.AddStep("Test stesp here", Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            var step = repo.All<Step>().ToList();
            var task = step.First().ActiveTask;
            var stepFK = step.First().TaskFK;
            var stepID = task.Steps.First().StepId;
            Assert.That(result.IsCompletedSuccessfully, Is.True);
            Assert.That(task.Id, Is.EqualTo(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391")));
            Assert.That(stepFK, Is.EqualTo(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391")));
            Assert.That(stepID, Is.Not.Null);
        }

        [Test]
        public void StepDetails()
        {
            var service = serviceProvider.GetService<ITaskService>();
            var result = service.AddStep("Test stesp here", Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            var stepData = repo.All<Step>().Where(x => x.TaskFK == Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391")).First();
            var step = new StepsViewModel
            {
                Step = stepData.Title
            };
 
            Assert.That(step.Step, Is.EqualTo("Test stesp here"));
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
            await repo.AddAsync(testTask);
            await repo.SaveChangesAsync();
        }
    }
}