using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Migrations;
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
            /* var task = await repo.All<ActiveTask>()
               .FirstOrDefaultAsync(t => t.Id.ToString() == taskId);*/
            var task = await repo.GetByIdAsync<ActiveTask>(taskId);
            //var f = await repo.All<Step>();
            var test =  new TaskViewModel()
            {
                Id = task.Id,
                Note = task.Note,
                DueDate = task.DueDate,
                IsImportant = task.IsImportant,  
                Steps = task.Steps
            };
            return test;
        }

        public void  GetFullTask()
        {
            var f = repo.All<Step>();
            var f1 = repo.All<Step>();
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
