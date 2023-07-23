using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly IApplicatioDbRepository repo;
        public TaskService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
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
            var activeTask = await repo.GetByIdAsync<ActiveTask>(taskId);
            var TaskSteps = repo.All<Step>().Where(x => x.TaskFK == taskId).ToList();
            var TaskStatenets = repo.All<Statement>().Where(x => x.TaskFK == taskId).ToList();

            return  new TaskViewModel()
            {
                Id = activeTask.Id,
                Note = activeTask.Note,
                DueDate = activeTask.DueDate,
                IsImportant = activeTask.IsImportant,
                Steps = TaskSteps,
                Statements = TaskStatenets
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

        public async Task AddStep(string stepText, Guid TaskId)
        {
            var task = await repo.GetByIdAsync<ActiveTask>(TaskId);

            var stepToAdd = new Step()
            {
                Title = stepText,
            };

            task.Steps.Add(stepToAdd);
            repo.SaveChanges();
        }

    }
}
