using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class TasksService : ITasksService
    {
        private readonly IApplicatioDbRepository repo;
        public TasksService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public TasksListViewModel GetTodayTasks(string userId)
        {
            var userTasks = GetPlannedTasks(userId);
            var test1 = userTasks.TaskViewModel.Where(x => x.DueDate == DateTime.Today).ToList();

            return new TasksListViewModel() { TaskViewModel = test1 };
        }

        public TasksListViewModel GetPlannedTasks(string userId)
        {
            var userTasks = GetAllOpenTasks(userId).Where(task => task.DueDate != null).ToList();

            return new TasksListViewModel()
            {TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        public TasksListViewModel GetImportantTasks(string userId)
        {
            var userTasks = GetAllOpenTasks(userId).Where(task => task.IsImportant == true).ToList();

            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        public TasksListViewModel GetAllTasks(string userId)
        {
            var userTasks = GetAllOpenTasks(userId);

            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        internal List<TaskViewModel> GetAllOpenTasks(string userId)
        {
            return repo.All<ActiveTask>()
              .Where(task => task.UserId == userId &&
              task.DueDate >= DateTime.Today)
              .Select(t => new TaskViewModel()
              {
                  Id = t.Id,
                  Note = t.Note,
                  DueDate = t.DueDate,
                  IsImportant = t.IsImportant
              }).ToList();
        }

    }
}
