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
            var stepsToRemove = repo.All<Step>()
                .Where(x => x.TaskFK == Id).ToList();
            repo.DeleteRange(stepsToRemove);

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
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        public async Task EditDate(TaskViewModel taskViewModel, string userId)
        {
            var taskToUpdate = await repo.GetByIdAsync<ActiveTask>(taskViewModel.Id);
            taskToUpdate.DueDate = taskViewModel.DueDate;
           /* var taskToUpdate = new ActiveTask
            {
                Id = taskViewModel.Id,
                Note = taskViewModel.Note,
                DueDate = taskViewModel.DueDate,
                IsImportant = taskViewModel.IsImportant,
                UserId = userId
            };*/

            repo.Update(taskToUpdate);
            repo.SaveChanges();
        }

        internal List<TaskViewModel> AllOverdueTasks(string userId)
        {
            var openTasks = repo.All<ActiveTask>()
              .Where(task => task.UserId == userId &&
              task.DueDate < DateTime.Today)
              .ToList();

            openTasks.ForEach(task =>
            {
                var currentSteps = repo.All<Step>().Where(x => x.TaskFK == task.Id).ToList();
                task.Steps = currentSteps;
            });

            var result = openTasks.Select(task => new TaskViewModel()
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

