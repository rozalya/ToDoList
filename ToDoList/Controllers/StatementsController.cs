using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Core.Contracts;

namespace ToDoList.Controllers
{
    [Authorize(Policy = "StatementsUserRolePolicy")]
    public class StatementsController : BaseController
    {
        private readonly IStatementService statementService;
        private readonly UserManager<IdentityUser> userManager;
        ILogger<StatementsController> logger;

        public StatementsController(IStatementService _statementService,
           UserManager<IdentityUser> _userManager,
            ILogger<StatementsController> _logger)
        {
            statementService = _statementService;
            userManager = _userManager;
            logger = _logger;
        }

        public IActionResult Index(string Id)
        {
            if (Id == null)
            {
                ViewBag.ErrorMessage = $"Task with Id = {Id} cannot be found";
                return View("NotFound");
            }

            _id = Id;
            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStatement(string IfText, string ThenText)
        {
            var taskId = Guid.Parse(_id);
            if(IfText != null && ThenText != null)
            {
                await statementService.AddStatement(IfText, ThenText, taskId);
                return RedirectToAction("Details","Task", new { Id = taskId });
            }
            return RedirectToAction("Index", "Statements", new { Id = taskId });
        }

        public static string _id { get; set; }
    }
}
