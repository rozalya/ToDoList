using System.Web;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Core.Services.CommonUtils;
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

        /// <summary>
        /// Add new task.
        /// </summary>
        /// <param name="taskViewModel"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task NewTask(TaskViewModel taskViewModel, string Id)
        {
            ActiveTask newtask = new ActiveTask
            {
                UserId = Id,
                Note = HttpUtility.HtmlEncode(taskViewModel.Note),
                DueDate = taskViewModel.DueDate,
                IsImportant = taskViewModel.IsImportant,
            };

            var result = repo.AddAsync(newtask);
            repo.SaveChanges();
            return result;
        }

        /// <summary>
        /// Edit current task.
        /// </summary>
        /// <param name="taskViewModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task EditTask(TaskViewModel taskViewModel, string userId)
        {
            var taskToEdit = await repo.GetByIdAsync<ActiveTask>(taskViewModel.Id);
            taskToEdit.Note = HttpUtility.HtmlEncode(taskViewModel.Note);
            taskToEdit.DueDate = taskViewModel.DueDate;
            taskToEdit.IsImportant = taskViewModel.IsImportant;

            repo.Update(taskToEdit);
            repo.SaveChanges();
        }

        /// <summary>
        /// Get the current task details.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<TaskViewModel> GetTask(Guid taskId)
        {
            var activeTask = await repo.GetByIdAsync<ActiveTask>(taskId);
            if(activeTask != null) 
            {
                var TaskSteps = repo.All<Step>().Where(x => x.TaskFK == taskId).ToList();
                var TaskStatenets = repo.All<Statement>().Where(x => x.TaskFK == taskId).ToList();

                return new TaskViewModel()
                {
                    Id = activeTask.Id,
                    Note = HttpUtility.HtmlDecode(activeTask.Note),
                    DueDate = activeTask.DueDate,
                    IsImportant = activeTask.IsImportant,
                    Steps = Common.DecodeSteps(TaskSteps),
                    Statements = Common.DecodeStatements(TaskStatenets),
                };
            }
          return null;
        }

        /// <summary>
        /// Delete the current task.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteTask(Guid Id)
        {
            await repo.DeleteAsync<ActiveTask>(Id);
            repo.SaveChanges();
        }

        /// <summary>
        /// Compleat the current task.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
            };

            await repo.AddAsync(doneTask);
            repo.SaveChanges();
            await repo.DeleteAsync<ActiveTask>(Id);
            repo.SaveChanges();
        }

        /// <summary>
        /// Add step to the current task.
        /// </summary>
        /// <param name="stepText"></param>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public async Task AddStep(string stepText, Guid TaskId)
        {
            var task = await repo.GetByIdAsync<ActiveTask>(TaskId);

            var stepToAdd = new Step()
            {
                Title = HttpUtility.HtmlEncode(stepText),
            };

            task.Steps.Add(stepToAdd);
            repo.SaveChanges();
        }

    }
}
