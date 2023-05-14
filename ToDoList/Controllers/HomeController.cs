using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using ToDoList.Core.Contracts;
using ToDoList.Core.Models;
using ToDoList.Core.Services;
using ToDoList.Infrastructure.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAddNewTaskService addNewTaskService;

        public HomeController(ILogger<HomeController> logger,
            IAddNewTaskService _addNewTaskService)
        {
            _logger = logger;
            addNewTaskService = _addNewTaskService;
        }

        [HttpGet]
        public IActionResult AddNewTask()
        {
            return View();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddNewTask(AddNewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    addNewTaskService.NewTask(model);
                }
                catch (ArgumentException ae)
                {
                    return BadRequest(ae.Message);
                }

                return Ok();               
            }

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}