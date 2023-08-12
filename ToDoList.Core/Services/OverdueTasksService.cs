using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Core.Services.CommonUtils;
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

        /// <summary>
        /// Close the current task.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all overdue tasks.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TasksListViewModel GetAllOverdueTasks(string userId)
        {
            var userTasks = AllOverdueTasks(userId);
            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        /// <summary>
        /// Edit the current task date.
        /// </summary>
        /// <param name="taskViewModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task EditDate(TaskViewModel taskViewModel, string userId)
        {
            var taskToUpdate = await repo.GetByIdAsync<ActiveTask>(taskViewModel.Id);
            taskToUpdate.DueDate = taskViewModel.DueDate;
            repo.Update(taskToUpdate);
            repo.SaveChanges();
        }

        /// <summary>
        /// Fet all overdue tasks.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal List<TaskViewModel> AllOverdueTasks(string userId)
        {
            var openTasks = repo.All<ActiveTask>()
              .Where(task => task.UserId == userId &&
              task.DueDate < DateTime.Today)
              .ToList();

            var decodedTasks = new List<ActiveTask>();

            openTasks.ForEach(task =>
            {
                decodedTasks.Add(Common.DecodeTask(task));
            });

            decodedTasks.ForEach(task =>
            {
                var currentSteps = repo.All<Step>().Where(x => x.TaskFK == task.Id).ToList();
                task.Steps = currentSteps;
                var currentStatemets = repo.All<Statement>().Where(x => x.TaskFK == task.Id).ToList();
                task.Statements = currentStatemets;
            });

            var result = decodedTasks.Select(task => new TaskViewModel()
            {
                Id = task.Id,
                Note = task.Note,
                DueDate = task.DueDate,
                IsImportant = task.IsImportant,
                Steps = task.Steps,
                Statements = task.Statements
            }).ToList();

            return result;
        }
    }

}

