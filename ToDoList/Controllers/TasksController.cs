using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;

namespace ToDoList.Controllers
{
    public class TasksController : BaseController
    {
        private readonly ITasksService tasksService;
        private readonly UserManager<IdentityUser> userManager;

        public TasksController(ITasksService _tasksService,
           UserManager<IdentityUser> _userManager)
        {
            tasksService = _tasksService;
            userManager = _userManager;
        }    
        public ActionResult AllTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = tasksService.GetAllTasks(userId);
            return View(model);
        }

        public ActionResult ImportantTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = tasksService.GetImportantTasks(userId);
            return View(model);
        }

        public ActionResult PlannedTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = tasksService.GetPlannedTasks(userId);
            return View(model);
        }
        public ActionResult TaskWithSteps()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = tasksService.GetTaskWithSteps(userId);
            return View(model);
        }
        public ActionResult TaskWithStatements()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = tasksService.GetTaskWithStatements(userId);
            return View(model);
        }

        public ActionResult TodayTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = tasksService.GetTodayTasks(userId);
            return View(model);
        }

        // POST: AddNewTaskController/Create

      
    }
}
