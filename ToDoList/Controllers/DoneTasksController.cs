using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    [Authorize(Policy = "InactiveTaskRolePolicy")]
    public class DoneTasksController : BaseController
    {
        private readonly IDoneTasksService doneTasksService;
        private readonly UserManager<IdentityUser> userManager;

        public DoneTasksController(IDoneTasksService _doneTasksService,
           UserManager<IdentityUser> _userManager)
        {
            doneTasksService = _doneTasksService;
            userManager = _userManager;
        }
        public IActionResult AllDoneTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = doneTasksService.GetAllDoneTasks(userId);
            return View(model);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var task = await doneTasksService.GetTask(id);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReopenTask(Guid Id)
        {
            try
            {
                await doneTasksService.ReopenTask(Id);
                return RedirectToAction("AllTasks", "OverdueTasks");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> RateTask(Guid id)
        {
            _id = id;
            var task = await doneTasksService.GetTask(id);
            if (task == null)
            {
                ViewBag.ErrorMessage = $"Task with Id = {task} cannot be found";
                return View("NotFound");
            }

            var model = new RateTaskViewModel
            {
                FirstStar = task.Rate == null ? 0 : task.Rate.FirstStar,
                SecondStar = task.Rate == null ? 0 : task.Rate.SecondStar,
                ThirdStar = task.Rate == null ? 0 : task.Rate.ThirdStar,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RateTask(RateTaskViewModel model)
        {
            var task = await doneTasksService.GetTask(_id);

            if (task == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {task.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                await doneTasksService.AddRate(model, _id);
                return RedirectToAction("Details", new { Id = task.Id });
            }
        }

        public static Guid _id { get; set; }
    }
}
