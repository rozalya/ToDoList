using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    public class DoneTasksController : BaseController
    {
        private readonly IDoneTasksService  doneTasksService;
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
    }
}
