using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class ExpiredTasksService : IExpiredTasksService
    {
        private readonly IApplicatioDbRepository repo;
        public ExpiredTasksService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }
        public TasksListViewModel GetAllExpiredTasks(string userId)
        {
            var userTasks =  repo.All<ExpiderTask>()
            .Where(task => task.UserId == userId)
            .Select(t => new TaskViewModel()
            {
                Id = t.Id,
                Note = t.Note,
                DueDate = t.DueDate,
                IsImportant = t.IsImportant,
            }).ToList();

            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        public async Task DeleteTask(Guid Id)
        {
            await repo.DeleteAsync<ExpiderTask>(Id);
            repo.SaveChanges();
        }
    }
}
