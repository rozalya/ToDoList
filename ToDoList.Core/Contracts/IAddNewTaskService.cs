using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface IAddNewTaskService
    {
        Task NewTask(AddNewTaskViewModel addNewTaskViewModel, string Id);

        TasksListViewModel GetAllTasks(string id);
    }
}
