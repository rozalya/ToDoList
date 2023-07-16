using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface ITasksService
    {
        Task NewTask(TaskViewModel taskViewModel, string userId);
        Task EditTask(TaskViewModel taskViewModel, string userId);

        TasksListViewModel GetAllTasks(string userId);
        TasksListViewModel GetImportantTasks(string userId);
        TasksListViewModel GetPlannedTasks(string userId);
        TasksListViewModel GetTodayTasks(string userId);
        Task <TaskViewModel> GetTask(Guid id);
        Task DeleteTask(Guid Id);
        Task CompleteTask(Guid Id);
    }
}
