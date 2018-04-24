using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManager.Data;
using EventManager.Models;
using EventManager.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManager.Controllers
{
    public class EventFinderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventItemService _eventItemService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventFinderController(IEventItemService eventItemService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _eventItemService = eventItemService;
            _userManager = userManager;
            _context = context;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            IEnumerable<EventItem>items = await _eventItemService.GetUpcomingEventsAsync();
            IEnumerable<ApplicationUser>users = _context.Users.Where(x => items.GetEnumerator().Current.HostUUID == x.Id);
            //Attempts to grab all Upcoming Events
            var events = new EventViewModel();
            events.Events = items;
            events.Users = users;

            //return Success
            return View(events);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            return Redirect("/Event/" + id + "/Details");
        }
        
    }
}
