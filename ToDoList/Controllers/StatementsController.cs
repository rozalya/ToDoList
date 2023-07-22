using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Core.Contracts;

namespace ToDoList.Controllers
{
    public class StatementsController : BaseController
    {
        public  string _Id; 
        private readonly IStatementService statementService;
        private readonly UserManager<IdentityUser> userManager;

        public StatementsController(IStatementService _statementService,
           UserManager<IdentityUser> _userManager)
        {
            statementService = _statementService;
            userManager = _userManager;
        }

        public IActionResult Index(string Id)
        {
            _Id = Id;
            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStatement(string IfText, string ThenText)
        {
            var taskId = _Id;
            //await statementService.AddStatement(IfText, ThenText, taskId);
            return RedirectToAction("Details", new { taskId });
        }
    }
}
