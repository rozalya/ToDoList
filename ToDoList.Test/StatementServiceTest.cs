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
    public class StatementServiceTest
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
                .AddSingleton<IStatementService, StatementService>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);
        }


        [Test]
        public void AddEditStatementToTask()
        {
            var service = serviceProvider.GetService<IStatementService>();
            var result = service.AddStatement("If state", "Then state", Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            var statemt = repo.All<Statement>().Where(x => x.TaskFK == (Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"))).First();

            Assert.That(result.IsCompletedSuccessfully, Is.True);
            Assert.That(statemt.If, Is.EqualTo("If state"));
            Assert.That(statemt.Then, Is.EqualTo("Then state"));

            statemt.If = "Edited if state";
            statemt.Then = "Edited then state";
            repo.SaveChanges();

            var statemt1 = repo.All<Statement>().Where(x => x.TaskFK == (Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"))).First();

            Assert.That(statemt1.If, Is.EqualTo("Edited if state"));
            Assert.That(statemt1.Then, Is.EqualTo("Edited then state"));           
        }

        [Test]
        public void DetailsTask()
        {
            var service = serviceProvider.GetService<IStatementService>();
            var result = service.AddStatement("If state", "Then state", Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            var statemt = repo.All<Statement>().Where(x => x.TaskFK == (Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"))).First();

            var state = new StatementsViewModel
            {
                If = statemt.If,
                Then = statemt.Then,
            };

            Assert.That(state.If, Is.EqualTo("If state"));
            Assert.That(state.Then, Is.EqualTo("Then state"));
        }


        [Test]
        public void GetTask()
        {
            var service = serviceProvider.GetService<IStatementService>();
            var result = service.AddStatement("If state", "Then state", Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391"));
            var state = repo.All<Statement>().ToList();
            var task = state.First().ActiveTask;
            var stateFK = state.First().TaskFK;
            var stateID = task.Statements.First().StatementId;
            Assert.That(result.IsCompletedSuccessfully, Is.True);
            Assert.That(task.Id, Is.EqualTo(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391")));
            Assert.That(stateFK, Is.EqualTo(Guid.Parse("fca6a9ac-2611-48df-b7d1-485fe4465391")));
            Assert.That(stateID, Is.Not.Null);
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
