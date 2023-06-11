using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class AddNewTaskService : IAddNewTaskService
    {
        private readonly IApplicatioDbRepository repo;
        public AddNewTaskService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public TasksListViewModel GetTodayTasks(string userId)
        {
            var userTasks = GetPlannedTasks(userId);
            var test1 = userTasks.AddNewTaskViewModel.Where(x => DateTime.Parse(x.DueDate).ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();

            var test = new TasksListViewModel() { AddNewTaskViewModel = test1 };
            return test;
        }

        public TasksListViewModel GetPlannedTasks(string userId)
        {
            var userTasks = repo.All<NewTask>()
                .Where(task => task.UserId == userId && task.DueDate != null)
                .Select(t => new AddNewTaskViewModel()
                {
                    Note = t.Note,
                    DueDate = t.DueDate,
                    IsImportant = t.IsImportant
                }).ToList();

            var test = new TasksListViewModel() { AddNewTaskViewModel = userTasks };
            return test;
        }

        public TasksListViewModel GetImportantTasks(string userId)
        {
            var userTasks = repo.All<NewTask>()
                .Where(task => task.UserId == userId && task.IsImportant == true)
                .Select(t => new AddNewTaskViewModel()
                {
                    Note = t.Note,
                    DueDate = t.DueDate,
                    IsImportant = t.IsImportant
                }).ToList();

            var test = new TasksListViewModel() { AddNewTaskViewModel = userTasks };
            return test;
        }

        public TasksListViewModel GetAllTasks(string userId)
        {
            var userTasks = repo.All<NewTask>()
                .Where(task => task.UserId == userId)
                .Select(t => new AddNewTaskViewModel()
                {
                    Note = t.Note,
                    DueDate = t.DueDate,
                    IsImportant = t.IsImportant
                }).ToList();

            var test = new TasksListViewModel() { AddNewTaskViewModel = userTasks };
            return test;
        }

        public Task NewTask(AddNewTaskViewModel addNewTaskViewModel, string Id)
        {

          /*  if (addNewTaskViewModel.DueDate != null)
            {
                date = DateTime.ParseExact(addNewTaskViewModel.DueDate, "g", new CultureInfo("en-US"), DateTimeStyles.None);
            }*/

            NewTask newtask = new NewTask
            {
                UserId = Id,
                Note = addNewTaskViewModel.Note,
                DueDate = addNewTaskViewModel.DueDate,
                IsImportant = addNewTaskViewModel.IsImportant,
            };

            var result = repo.AddAsync(newtask);
              repo.SaveChanges();

            return result;
        }
    }
}
