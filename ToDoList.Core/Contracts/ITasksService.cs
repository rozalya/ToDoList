using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface ITasksService
    {      
        TasksListViewModel GetAllTasks(string userId);
        TasksListViewModel GetImportantTasks(string userId);
        TasksListViewModel GetPlannedTasks(string userId);
        TasksListViewModel GetTodayTasks(string userId);    
        TasksListViewModel GetTaskWithSteps(string userId);    
        TasksListViewModel GetTaskWithStatements(string userId);    
    }
}
