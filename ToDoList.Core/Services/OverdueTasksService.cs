using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class OverdueTasksService : IOverdueTasksService
    {
        private readonly IApplicatioDbRepository repo;
        public OverdueTasksService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task CloseTask(Guid Id)
        {
            var taskToClose = await repo.GetByIdAsync<ActiveTask>(Id);
            var expiredTask = new ExpiderTask
            {
                Id = taskToClose.Id,
                UserId = taskToClose.UserId,
                Note = taskToClose.Note,
                DueDate = taskToClose.DueDate,
                IsImportant = taskToClose.IsImportant,
            };

            await repo.AddAsync(
                expiredTask);
            repo.SaveChanges();
            await repo.DeleteAsync<ActiveTask>(Id);
            repo.SaveChanges();
        }

        public TasksListViewModel GetAllOverdueTasks(string userId)
        {
            var userTasks = AllOverdueTasks(userId);
            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.Result.OrderBy(x => x.DueDate).ToList()
            };
        }
        internal async Task<List<TaskViewModel>> AllOverdueTasks(string userId)
        {
            var userTasks = await repo.All<ActiveTask>()
              .Where(task => task.UserId == userId &&
              task.DueDate < DateTime.Today)
              .Select(t => new TaskViewModel()
              {
                  Id = t.Id,
                  Note = t.Note,
                  DueDate = t.DueDate,
                  IsImportant = t.IsImportant,
              }).ToListAsync();

            return userTasks;
        }
    }

}

