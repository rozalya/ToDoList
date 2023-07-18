using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface IExpiredTasksService
    {
        TasksListViewModel GetAllExpiredTasks(string userId);
        Task DeleteTask(Guid Id);
    }
}
