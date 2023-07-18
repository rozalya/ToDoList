using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface IOverdueTasksService
    {
       TasksListViewModel GetAllOverdueTasks(string userId);
       Task CloseTask(Guid Id);
       Task EditDate(TaskViewModel taskViewModel, string userId);
    }
}
