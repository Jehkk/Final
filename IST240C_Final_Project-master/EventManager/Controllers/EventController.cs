using EventManager.Data;
using EventManager.Models;
using EventManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EventManager.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventItemService _eventItemService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventController(IEventItemService eventItemService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _eventItemService = eventItemService;
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Create Event View for users
        /// </summary>
        /// <returns>Create Event View</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This is used to actually create an event by passing in a NewEventItem or the required
        /// Components to make a NewEventItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Event Id of newly created event. You can use it like a string. </returns>
        [HttpPost]
        public async Task<IActionResult> CreateEvent(NewEventItem item)
        {
            //Check to make sure the correct item model was passed in
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //Validate user
            if (await _userManager.GetUserAsync(User) == null) { return Unauthorized(); }

            //attempt to add event
            var successful = await _eventItemService.AddEventAsync(item, await _userManager.GetUserAsync(User));

            //check if the variable successfull is an event item
            if (!(successful is EventItem)) { return BadRequest(new { error = "Could not edit item." }); }

            //all went well so we are returning the new guid
            return Json(successful.Id.ToString());
        }

        /// <summary>
        /// Delete Confirmation for user
        /// </summary>
        /// <returns>Delete Confirmation View</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            //return 404 if guid is invalid
            if (id == null) { return NotFound(); }
            
            //Attempt to grab Event based on id
            var eventItem = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);

            //Return 404 if no event was found
            if (eventItem == null) { return NotFound(); }

            //return to the management page
            return View(eventItem);
        }

        /// <summary>
        /// Deletes a Users Event specified by EventID
        /// </summary>
        /// <returns>EventManagement Index</returns>
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //validate user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            //Attempts to delete Event
            var successful = await _eventItemService.RemoveEventAsync(id, currentUser);

            //Returns Error
            if (!successful){return BadRequest(new { error = "Could not remove Event." });}

            //Returns success
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// An View for Viewing a specific event and performing actions based on whos viewing
        /// </summary>
        /// <returns>Single Event View</returns>
        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            //return 404 if guid is invalid
            if (id == null){return NotFound();}

            //Attempt to grab Event based on id
            var eventItem = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);

            //return 404 if event is invalid
            if (eventItem == null){return NotFound();}

            //returns view of specified event
            return View(eventItem);
        }

        /// <summary>
        /// A View for editing a specific event
        /// </summary>
        /// <returns>Event edit View</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            //return 404 if guid is invalid
            if (id == null) { return NotFound(); }

            //validate user
            var currentUser = await _userManager.GetUserAsync(User);

            //attempts to grab specified event
            var eventItem = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);

            //return Error
            if (eventItem == null) { return NotFound(); }

            //Return Success
            return View(eventItem);
        }

        /// <summary>
        /// Post Request to edit Events
        /// </summary>
        /// <returns>BadRequest along with the error messege</returns>
        /// <returns>ok</returns>
        [HttpPost]
        public async Task<IActionResult> EditItem(Guid id, NewEventItem item)
        {
            //Check to make sure the correct item model was passed in
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //Validate user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            //attempts to grab specified event
            var successful = await _eventItemService.EditEventAsync(id, item, currentUser);

            //return Error
            if (!successful){return BadRequest(new { error = "Could not edit item." });}

            //return Success
            return Ok();
        }

        /// <summary>
        /// Displays all of the users events they have created and are participating in along with some actions
        /// </summary>
        /// <returns>User EventManager</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Validate User
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            //Attempts to grab users Events
            var events = await _eventItemService.GetIncompleteItemsAsync(currentUser);

            //return Success
            return View(events);
        }
    }
}