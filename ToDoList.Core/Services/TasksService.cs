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
            var test1 = userTasks.AddNewTaskViewModel.Where(x => x.DueDate == DateTime.Today).ToList();

            return new TasksListViewModel() { AddNewTaskViewModel = test1 };
        }

        public TasksListViewModel GetPlannedTasks(string userId)
        {
            var userTasks = repo.All<NewTask>()
                .Where(task => task.UserId == userId && task.DueDate != null)
                .Select(t => new AddNewTaskViewModel()
                {
                    Id = t.Id,
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
                    Id = t.Id,
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
                    Id = t.Id,
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

        public void EditTask(AddNewTaskViewModel addNewTaskViewModel, string userId)
        {
            NewTask newtask = new NewTask
            {
                Id = addNewTaskViewModel.Id,
                Note = addNewTaskViewModel.Note,
                DueDate = addNewTaskViewModel.DueDate,
                IsImportant = addNewTaskViewModel.IsImportant,
                UserId = userId
            };

            repo.Update(newtask);
            repo.SaveChanges();
        }

        public async Task<AddNewTaskViewModel> GetTask(string taskId)
        {
            var task = await repo.All<NewTask>()
              .FirstOrDefaultAsync(t => t.Id.ToString() == taskId);

            return new AddNewTaskViewModel()
            {
                Id = task.Id,
                Note = task.Note,
                DueDate = task.DueDate,
                IsImportant = task.IsImportant
            };
        }

        public async Task DeleteTask(Guid Id)
        {
            await repo.DeleteAsync<NewTask>(Id);
            repo.SaveChanges();
        }
    }
}
