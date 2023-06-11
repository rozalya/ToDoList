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
            var test1 = userTasks.AddNewTaskViewModel.Where(x => x.DueDate == DateTime.Today).ToList();

            return new TasksListViewModel() { AddNewTaskViewModel = test1 };
        }

        public TasksListViewModel GetPlannedTasks(string userId)
        {
            var userTasks = repo.All<NewTask>()
                .Where(task => task.UserId == userId && task.DueDate != null)
                .Select(t => new AddNewTaskViewModel()
                {
                    Note = t.Note,
                    DueDate = t.DueDate,
                    IsImportant = t.IsImportant
                }).ToList();

            return new TasksListViewModel()
            {
                AddNewTaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        public TasksListViewModel GetImportantTasks(string userId)
        {
            var userTasks = repo.All<NewTask>()
                .Where(task => task.UserId == userId && task.IsImportant == true)
                .Select(t => new AddNewTaskViewModel()
                {
                    Note = t.Note,
                    DueDate = t.DueDate,
                    IsImportant = t.IsImportant
                }).ToList();

            return new TasksListViewModel()
            {
                AddNewTaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        public TasksListViewModel GetAllTasks(string userId)
        {
            var userTasks = repo.All<NewTask>()
                .Where(task => task.UserId == userId)
                .Select(t => new AddNewTaskViewModel()
                {
                    Note = t.Note,
                    DueDate = t.DueDate,
                    IsImportant = t.IsImportant
                }).ToList();

            return new TasksListViewModel()
            {
                AddNewTaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        public Task NewTask(AddNewTaskViewModel addNewTaskViewModel, string Id)
        {
            NewTask newtask = new NewTask
            {
                UserId = Id,
                Note = addNewTaskViewModel.Note,
                DueDate = addNewTaskViewModel.DueDate,
                IsImportant = addNewTaskViewModel.IsImportant,
            };

            var result = repo.AddAsync(newtask);
            repo.SaveChanges();

            return result;
        }
    }
}
