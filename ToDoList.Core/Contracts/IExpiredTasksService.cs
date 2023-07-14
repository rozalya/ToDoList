using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Core.Contracts
{
    public interface IExpiredTasksService
    {
       TasksListViewModel GetAllExpiredTasks(string userId);
       Task CloseTask(Guid Id);
    }
}
