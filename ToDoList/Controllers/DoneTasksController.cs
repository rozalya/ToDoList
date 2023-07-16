using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;

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
    }
}
