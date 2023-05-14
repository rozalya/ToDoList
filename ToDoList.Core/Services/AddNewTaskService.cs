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
        public Task NewTask(AddNewTaskViewModel addNewTaskViewModel)
        {

          /*  if (addNewTaskViewModel.DueDate != null)
            {
                date = DateTime.ParseExact(addNewTaskViewModel.DueDate, "g", new CultureInfo("en-US"), DateTimeStyles.None);
            }*/

            NewTask newtask = new NewTask
            {
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
