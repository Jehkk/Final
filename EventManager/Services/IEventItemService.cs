using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManager.Models;

namespace EventManager.Services
{
    public interface IEventItemService
    {
        Task<IEnumerable<EventItem>> GetIncompleteItemsAsync(ApplicationUser user);

        Task<IEnumerable<EventItem>> GetUpcomingEventsAsync();

        Task<EventItem> AddEventAsync(NewEventItem newItem, ApplicationUser user);

        Task<bool> EditEventAsync(Guid id, NewEventItem newItem, ApplicationUser user);

        Task<bool> RemoveEventAsync(Guid id, ApplicationUser user);


    }
}
