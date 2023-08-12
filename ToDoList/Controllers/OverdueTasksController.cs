using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    public class OverdueTasksController : BaseController
    {
        private readonly IOverdueTasksService overdueTasks;
        private readonly ITaskService taskService;
        private readonly UserManager<IdentityUser> userManager;
        ILogger<OverdueTasksController> logger;

        public OverdueTasksController(IOverdueTasksService _overdueTasks,
           UserManager<IdentityUser> _userManager,
           ITaskService _taskService,
           ILogger<OverdueTasksController> _logger)
        {
            overdueTasks = _overdueTasks;
            userManager = _userManager;
            taskService = _taskService;
            logger = _logger;
        }
        public IActionResult AllTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = overdueTasks.GetAllOverdueTasks(userId);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CloseTask(Guid Id)
        {
            try
            {
                await overdueTasks.CloseTask(Id);
                return RedirectToAction("AllTasks");
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var task = await taskService.GetTask(id);
            if (task == null)
            {
                ViewBag.ErrorMessage = $"Task with Id = {task} cannot be found";
                return View("NotFound");
            }
            return View(task);
        }
        public async Task<IActionResult> EditDate(Guid Id)
        {
            var task = await taskService.GetTask(Id);
            if (task == null)
            {
                ViewBag.ErrorMessage = $"Task with Id = {task} cannot be found";
                return View("NotFound");
            }        
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDate(TaskViewModel model)
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
                    await overdueTasks.EditDate(model, userId);
                    return RedirectToAction("AllTasks");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }
    }
}
