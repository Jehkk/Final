using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EventManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Controllers
{
    [Authorize(Roles = Constants.AdministratorRole)]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> EventManager;
        
        public ManageUsersController(UserManager<ApplicationUser> userManager)
        {
            EventManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await EventManager
                .GetUsersInRoleAsync(Constants.AdministratorRole);

            var everyone = await EventManager.Users
                .ToArrayAsync();

            var model = new ManageUsersViewModel
            {
                Administrators = admins,
                Everyone = everyone
            };

            return View(model);
        }
    }
}
