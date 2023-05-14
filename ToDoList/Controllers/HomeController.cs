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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

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