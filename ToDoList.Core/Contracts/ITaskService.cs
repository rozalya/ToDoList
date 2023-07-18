using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface ITaskService
    {
        Task NewTask(TaskViewModel taskViewModel, string userId);
        Task EditTask(TaskViewModel taskViewModel, string userId);
        Task<TaskViewModel> GetTask(Guid id);
        Task DeleteTask(Guid Id);
        Task CompleteTask(Guid Id);
    }
}
