using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class ExpiredTasksService : IExpiredTasksService
    {
        private readonly IApplicatioDbRepository repo;
        public ExpiredTasksService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task CloseTask(Guid Id)
        {
            var taskToClose = await repo.All<NewTask>().FirstAsync(x => x.Id == Id);
            taskToClose.IsClosed = true;
            repo.Update(taskToClose);
            repo.SaveChanges();
        }

        public TasksListViewModel GetAllExpiredTasks(string userId)
        {
            var userTasks = AllExpiredTasks(userId);
            return new TasksListViewModel()
            {
                AddNewTaskViewModel = userTasks.Result.OrderBy(x => x.DueDate).ToList()
            };
        }
        internal async Task<List<AddNewTaskViewModel>> AllExpiredTasks(string userId)
        {
            var userTasks = await repo.All<NewTask>()
              .Where(task => task.UserId == userId &&
              task.DueDate != null &&
               task.DueDate.Value.CompareTo(DateTime.Now) < 0)
              .Select(t => new AddNewTaskViewModel()
              {
                  Id = t.Id,
                  Note = t.Note,
                  DueDate = t.DueDate,
                  IsImportant = t.IsImportant,
                  IsColsed = t.IsClosed
              }).ToListAsync();

            return userTasks;
        }
    }

}

