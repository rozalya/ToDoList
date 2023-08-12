using System.Web;
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

        /// <summary>
        /// Get all Done tasks  for the given  user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DoneTaskListViewModel GetAllDoneTasks(string userId)
        {
            var userTasks = repo.All<DoneTask>()
            .Where(task => task.UserId == userId)
            .Select(t => new DoneTaskViewModel()
            {
                Id = t.Id,
                Note = HttpUtility.HtmlDecode(t.Note),
                DueDate = t.DueDate,
                IsImportant = t.IsImportant,
            }).ToList();

            return new DoneTaskListViewModel()
            {
                DoneTaskListViewModels = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        /// <summary>
        /// Reopen the current task.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task ReopenTask(Guid Id)
        {
            var taskToReopen = await repo.GetByIdAsync<DoneTask>(Id);
            var activeTask = new ActiveTask
            {
                Id = taskToReopen.Id,
                UserId = taskToReopen.UserId,
                Note = HttpUtility.HtmlDecode(taskToReopen.Note),
                DueDate = taskToReopen.DueDate,
                IsImportant = taskToReopen.IsImportant,
            };

            await repo.AddAsync(activeTask);
            repo.SaveChanges();
            await repo.DeleteAsync<DoneTask>(Id);
            repo.SaveChanges();
        }

        /// <summary>
        /// Get the details of the current task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<DoneTaskViewModel> GetTask(Guid taskId)
        {
            var task = await repo.GetByIdAsync<DoneTask>(taskId);
            var taskRate = repo.All<Rate>().Where(x => x.TaskFK == taskId).FirstOrDefault();
            return new DoneTaskViewModel()
            {
                Id = task.Id,
                Note = HttpUtility.HtmlDecode(task.Note),
                DueDate = task.DueDate.Value,
                IsImportant = task.IsImportant,
                Rate = taskRate
            };
        }


        /// <summary>
        /// Add rate for the current task.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
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
