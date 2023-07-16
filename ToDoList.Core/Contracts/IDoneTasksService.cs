using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface IDoneTasksService
    {
        TasksListViewModel GetAllDoneTasks(string userId);
    }
}
