using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    public class TaskController : BaseController
    {
        private readonly ITaskService taskService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<TaskController> logger;

        public TaskController(ITaskService _taskService,
           UserManager<IdentityUser> _userManager,
            ILogger<TaskController> _logger)
        {
            taskService = _taskService;
            userManager = _userManager;
            logger = _logger;
        }

        [HttpGet]
        public IActionResult AddNewTask()
        {
            return View();
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var task = await taskService.GetTask(id);
            if(task == null)
            {
                ViewBag.ErrorMessage = $"Task with Id = {id} cannot be found";
                return View("NotFound");
            }
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
                    if (userId == null)
                    {
                        ViewBag.ErrorMessage = $"Role with Id = {userId} cannot be found";
                        return View("NotFound");
                    }
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

        public async Task<IActionResult> EditTask(Guid Id)
        {
            var task = await taskService.GetTask(Id);
            if (task == null)
            {
                ViewBag.ErrorMessage = $"Task with Id = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                return View(task);
            }
        }

        public async Task<IActionResult> GetSteps(Guid Id)
        {
            var task = await taskService.GetTask(Id);

            if (task == null)
            {
                ViewBag.ErrorMessage = $"Task with Id = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                return View(task);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(TaskViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (userId == null)
                    {
                        ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                        return View("NotFound");
                    }
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
        public async Task<IActionResult> AddNewStep(string Step, Guid Id)
        {
            if (Step != null)
            {
                    try
                    {
                        await taskService.AddStep(Step, Id);
                        return RedirectToAction("Details", new { Id });
                    }
                    catch (ArgumentException ae)
                    {
                        return BadRequest(ae.Message);
                    }              
                return RedirectToAction("Details", new { Id }); ;
            }

            return RedirectToAction("Details", new { Id });
        }
    }
}
