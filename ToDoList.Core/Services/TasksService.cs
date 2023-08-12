using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Core.Services.CommonUtils;
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

        /// <summary>
        /// Get all task with due date today for the given  user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TasksListViewModel GetTodayTasks(string userId)
        {
            var userTasks = GetPlannedTasks(userId);
            var todayTasks = userTasks.TaskViewModel.Where(x => x.DueDate == DateTime.Today).ToList();

            return new TasksListViewModel() { TaskViewModel = todayTasks };
        }

     //remove???
        public TasksListViewModel GetPlannedTasks(string userId)
        {
            var userTasks = GetAllOpenTasks(userId).Where(task => task.DueDate != null).ToList();

            return new TasksListViewModel()
            {TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }


        /// <summary>
        /// Get all tasks marked as important or the given  user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TasksListViewModel GetImportantTasks(string userId)
        {
            var userTasks = GetAllOpenTasks(userId).Where(task => task.IsImportant == true).ToList();

            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        /// <summary>
        /// Get all tasks that have Steps for the given  user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TasksListViewModel GetTaskWithSteps(string userId)
        {
            var userTasks = GetAllOpenTasks(userId).Where(task => task.Steps.Count > 0).ToList();

            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        /// <summary>
        /// Get all tasks that have Statements for the given  user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TasksListViewModel GetTaskWithStatements(string userId)
        {
            var userTasks = GetAllOpenTasks(userId).Where(task => task.Statements.Count > 0).ToList();

            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        /// <summary>
        /// Get all aktive tasks for the given  user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TasksListViewModel GetAllTasks(string userId)
        {
            var userTasks = GetAllOpenTasks(userId);

            return new TasksListViewModel()
            {
                TaskViewModel = userTasks.OrderBy(x => x.DueDate).ToList()
            };
        }

        /// <summary>
        /// Get all tasks  for the given  user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal List<TaskViewModel> GetAllOpenTasks(string userId)
        {
            var openTasks = repo.All<ActiveTask>()
              .Where(task => task.UserId == userId)
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
