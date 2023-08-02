using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface IDoneTasksService
    {
        DoneTaskListViewModel GetAllDoneTasks(string userId);
        Task ReopenTask(Guid Id);
        Task<DoneTaskViewModel> GetTask(Guid id);
        Task AddRate(RateTaskViewModel model, Guid taskId);
    }
}
