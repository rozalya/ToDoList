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

        public Task NewTask(TaskViewModel taskViewModel, string Id)
        {
            ActiveTask newtask = new ActiveTask
            {
                UserId = Id,
                Note = taskViewModel.Note,
                DueDate = taskViewModel.DueDate,
                IsImportant = taskViewModel.IsImportant,
            };

            var result = repo.AddAsync(newtask);
            repo.SaveChanges();

            return result;
        }

        public async Task EditTask(TaskViewModel taskViewModel, string userId)
        {
            //var taskToEdit = await repo.GetByIdAsync<ActiveTask>(taskViewModel.Id);
            var taskToUpdate = new ActiveTask
            {
                Id = taskViewModel.Id,
                Note = taskViewModel.Note,
                DueDate = taskViewModel.DueDate,
                IsImportant = taskViewModel.IsImportant,
                UserId = userId
            };

            repo.Update(taskToUpdate);
            repo.SaveChanges();
        }

        public async Task<TaskViewModel> GetTask(Guid taskId)
        {
            /* var task = await repo.All<ActiveTask>()
               .FirstOrDefaultAsync(t => t.Id.ToString() == taskId);*/
            var task = await repo.GetByIdAsync<ActiveTask>(taskId);
            return new TaskViewModel()
            {
                Id = task.Id,
                Note = task.Note,
                DueDate = task.DueDate,
                IsImportant = task.IsImportant
            };
        }

        public async Task DeleteTask(Guid Id)
        {
            await repo.DeleteAsync<ActiveTask>(Id);
            repo.SaveChanges();
        }

        public async Task CompleteTask(Guid Id)
        {
            var taskToClose = await repo.GetByIdAsync<ActiveTask>(Id);
            var doneTask = new DoneTask
            {
                Id = taskToClose.Id,
                UserId = taskToClose.UserId,
                Note = taskToClose.Note,
                DueDate = taskToClose.DueDate,
                CompletedDate = DateTime.Today,
                IsImportant = taskToClose.IsImportant,
                ClosingStatus = "Task is Done"
            };

           await repo.AddAsync(doneTask);
           repo.SaveChanges();
           await repo.DeleteAsync<ActiveTask>(Id);
           repo.SaveChanges();
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
