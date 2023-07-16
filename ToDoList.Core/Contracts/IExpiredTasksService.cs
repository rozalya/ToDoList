using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;

namespace ToDoList.Core.Contracts
{
    public interface IExpiredTasksService
    {
        TasksListViewModel GetAllExpiredTasks(string userId);
    }
}
