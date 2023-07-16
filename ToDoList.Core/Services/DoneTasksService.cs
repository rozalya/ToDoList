using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class DoneTasksService : IDoneTasksService
    {
        private readonly IApplicatioDbRepository repo;
        public DoneTasksService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }
        public TasksListViewModel GetAllDoneTasks(string userId)
        {
            var userTasks = repo.All<DoneTask>()
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
    }
}
