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
            var openTasks = repo.All<ActiveTask>()
              .Where(task => task.UserId == userId &&
              task.DueDate >= DateTime.Today)
              .ToList();

            openTasks.ForEach(task =>
            {
                var currentSteps = repo.All<Step>().Where(x => x.TaskFK == task.Id).ToList();
                task.Steps = currentSteps;
            });

           var result =  openTasks.Select(task => new TaskViewModel()
            {
                Id = task.Id,
                Note = task.Note,
                DueDate = task.DueDate,
                IsImportant = task.IsImportant,
                Steps = task.Steps
              
            }).ToList();

            return result;
        }

    }
}
