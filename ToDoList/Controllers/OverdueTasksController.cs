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
        private readonly UserManager<IdentityUser> userManager;

        public OverdueTasksController(IOverdueTasksService _overdueTasks,
           UserManager<IdentityUser> _userManager)
        {
            overdueTasks = _overdueTasks;
            userManager = _userManager;
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
    }
}
