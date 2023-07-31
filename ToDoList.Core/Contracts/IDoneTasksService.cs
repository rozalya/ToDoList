using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface IDoneTasksService
    {
        TasksListViewModel GetAllDoneTasks(string userId);
        Task ReopenTask(Guid Id);
        Task<TaskViewModel> GetTask(Guid id);
        Task AddRate(RateTaskViewModel model, Guid taskId);
    }
}
