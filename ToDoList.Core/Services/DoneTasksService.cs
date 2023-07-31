using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class DoneTasksService : IDoneTasksService
    {
        private readonly IApplicatioDbRepository repo;
        public DoneTasksService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }
        public TasksListViewModel GetAllDoneTasks(string userId)
        {
            var userTasks = repo.All<DoneTask>()
            .Where(task => task.UserId == userId)
            .Select(t => new TaskViewModel()
            {
                Id = t.Id,
                Note = t.Note,
                DueDate = t.DueDate,
                IsImportant = t.IsImportant,
            }).ToList();

            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        public async Task ReopenTask(Guid Id)
        {
            var taskToReopen = await repo.GetByIdAsync<DoneTask>(Id);
            var activeTask = new ActiveTask
            {
                Id = taskToReopen.Id,
                UserId = taskToReopen.UserId,
                Note = taskToReopen.Note,
                DueDate = taskToReopen.DueDate,
                IsImportant = taskToReopen.IsImportant,
            };

            await repo.AddAsync(
                activeTask);
            repo.SaveChanges();
            await repo.DeleteAsync<DoneTask>(Id);
            repo.SaveChanges();
        }

        public async Task<TaskViewModel> GetTask(Guid taskId)
        {
            var task = await repo.GetByIdAsync<DoneTask>(taskId);
            var taskRate = repo.All<Rate>().Where(x => x.TaskFK == taskId).FirstOrDefault();
            return new TaskViewModel()
            {
                Id = task.Id,
                Note = task.Note,
                DueDate = task.DueDate.Value,
                IsImportant = task.IsImportant,
                Rate = taskRate
            };
        }

        public async Task AddRate(RateTaskViewModel model, Guid taskId)
        {
            var ratedModel = new Rate
            {
                FirstStar = model.FirstStar,
                SecondStar = model.SecondStar,
                ThirdStar = model.ThirdStar,
                TaskFK = taskId
            };

            await repo.AddAsync(ratedModel);
            repo.SaveChanges();
        }
    }
}
