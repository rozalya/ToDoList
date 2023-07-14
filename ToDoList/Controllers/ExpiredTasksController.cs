using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    public class ExpiredTasksController : BaseController
    {
        private readonly IExpiredTasksService expiredTasks;
        private readonly UserManager<IdentityUser> userManager;

        public ExpiredTasksController(IExpiredTasksService _expiredTasks,
           UserManager<IdentityUser> _userManager)
        {
            expiredTasks = _expiredTasks;
            userManager = _userManager;
        }
        public IActionResult AllTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = expiredTasks.GetAllExpiredTasks(userId);
            return View(model);;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CloseTask(Guid Id)
        {
            try
            {
                await expiredTasks.CloseTask(Id);
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
