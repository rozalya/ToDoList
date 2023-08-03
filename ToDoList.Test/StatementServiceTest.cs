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
    public class StatementServiceTest
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
                .AddSingleton<IStatementService, StatementService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);
        }


        [Test]
        public void AddStatementToTask()
        {
            var service = serviceProvider.GetService<IStatementService>();
            var result = service.AddStatement("If state", "Then state", Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
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
            await repo.AddAsync(testTask);
            await repo.SaveChangesAsync();
        }
    }
}
