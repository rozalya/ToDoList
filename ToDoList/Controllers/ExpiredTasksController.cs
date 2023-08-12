using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;

namespace ToDoList.Controllers
{
    [Authorize(Policy = "InactiveTaskRolePolicy")]
    public class ExpiredTasksController : BaseController
    {
        private readonly IExpiredTasksService expiredTasksService;
        private readonly UserManager<IdentityUser> userManager;
        ILogger<ExpiredTasksController> logger;

        public ExpiredTasksController(IExpiredTasksService _expiredTasksService,
           UserManager<IdentityUser> _userManager,
           ILogger<ExpiredTasksController> _logger)
        {
            expiredTasksService = _expiredTasksService;
            userManager = _userManager;
            logger = _logger;
        }
        public IActionResult AllExpiredTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            var model = expiredTasksService.GetAllExpiredTasks(userId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTask(Guid Id)
        {
            try
            {
                await expiredTasksService.DeleteTask(Id);
                return RedirectToAction("AllExpiredTasks");
            }
            catch
            {
                return View();
            }
        }
    }
}
