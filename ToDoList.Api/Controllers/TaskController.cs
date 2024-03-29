using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Controllers;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : BaseController
    {
        private readonly ITaskService taskService;
        private readonly UserManager<IdentityUser> userManager;

        public TaskController(ITaskService _taskService,
           UserManager<IdentityUser> _userManager)
        {
            taskService = _taskService;
            userManager = _userManager;
        }

        // GET: AddNewTaskController
        [HttpGet]
        [Route("AddNew")]
        public IActionResult AddNewTask()
        {
            return View();
        }

        // GET: AddNewTaskController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var task = await taskService.GetTask(id);
            return View(task);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddNewTask(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    taskService.NewTask(model, userId);
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
        public async Task<IActionResult> EditTask(Guid Id)
        {
            var task = await taskService.GetTask(Id);
            return View(task);
        }

        public async Task<IActionResult> GetSteps(Guid Id)
        {
            var task = await taskService.GetTask(Id);

            return View(task);
        }

        // POST: AddNewTaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(TaskViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    await taskService.EditTask(model, userId);
                    return RedirectToAction("Details", new { Id = model.Id });
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTask(Guid Id)
        {
            try
            {
                await taskService.DeleteTask(Id);
                return RedirectToAction("AllTasks", "Tasks");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteTask(Guid Id)
        {
            try
            {
                await taskService.CompleteTask(Id);
                return RedirectToAction("AllDoneTasks", "DoneTasks");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "StepsUserRolePolicy")]
        public async Task<IActionResult> AddNewStep(string Note, Guid Id)
        {
            await taskService.AddStep(Note, Id);
            return RedirectToAction("Details", new { Id });
        }
    }


}