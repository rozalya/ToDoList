﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Contracts;

namespace ToDoList.Controllers
{
    public class ExpiredTasksController : BaseController
    {
        private readonly IExpiredTasksService expiredTasksService;
        private readonly UserManager<IdentityUser> userManager;

        public ExpiredTasksController(IExpiredTasksService _expiredTasksService,
           UserManager<IdentityUser> _userManager)
        {
            expiredTasksService = _expiredTasksService;
            userManager = _userManager;
        }
        public IActionResult AllExpiredTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = expiredTasksService.GetAllExpiredTasks(userId);
            return View(model);
        }
    }
}
