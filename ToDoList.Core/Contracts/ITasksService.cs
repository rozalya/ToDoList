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
        Task NewTask(AddNewTaskViewModel addNewTaskViewModel, string userId);
        void EditTask(AddNewTaskViewModel addNewTaskViewModel, string userId);

        TasksListViewModel GetAllTasks(string userId);
        TasksListViewModel GetImportantTasks(string userId);
        TasksListViewModel GetPlannedTasks(string userId);
        TasksListViewModel GetTodayTasks(string userId);
        Task <AddNewTaskViewModel> GetTask(string id);
        Task DeleteTask(Guid Id);
    }
}
