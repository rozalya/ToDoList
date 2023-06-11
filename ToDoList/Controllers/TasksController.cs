using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITasksService addNewTaskService;
        private readonly UserManager<IdentityUser> userManager;

        public TasksController(ITasksService _addNewTaskService,
           UserManager<IdentityUser> _userManager)
        {
            addNewTaskService = _addNewTaskService;
            userManager = _userManager;
        }

        // GET: AddNewTaskController
        [HttpGet]
        public IActionResult AddNewTask()
        {
            return View();
        }

        // GET: AddNewTaskController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult AllTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = addNewTaskService.GetAllTasks(userId);
            return View(model);
        }

        public ActionResult ImportantTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = addNewTaskService.GetImportantTasks(userId);
            return View(model);
        }

        public ActionResult PlannedTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = addNewTaskService.GetPlannedTasks(userId);
            return View(model);
        }

        public ActionResult TodayTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = addNewTaskService.GetTodayTasks(userId);
            return View(model);
        }

        // POST: AddNewTaskController/Create

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddNewTask(AddNewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    addNewTaskService.NewTask(model, userId);
                }
                catch (ArgumentException ae)
                {
                    return BadRequest(ae.Message);
                }

                return View("~/Views/Home/Index.cshtml");
            }

            return View();
        }

        // GET: AddNewTaskController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddNewTaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddNewTaskController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddNewTaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
