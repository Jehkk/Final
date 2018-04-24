using EventManager.Data;
using EventManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Services
{
    public class EventItemService : IEventItemService
    {
        private readonly ApplicationDbContext _context;

        public EventItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EventItem> AddEventAsync(NewEventItem newItem, ApplicationUser user)
        {
            var entity = new EventItem
            {
                Id = Guid.NewGuid(),
                HostUUID = user.Id,
                EventName = newItem.Name,
                EventDescription = newItem.Description,
                EventTime = newItem.EventTime,
                EventCreated = DateTimeOffset.Now.DateTime,
                IsDeleted = false
            };
            _context.Events.Add(entity);

            var saveResult = await _context.SaveChangesAsync();
            if(saveResult == 1)
            {
                return entity;
            }
            return null;
        }

        public async Task<bool> EditEventAsync(Guid id, NewEventItem newItem, ApplicationUser user)
        {
            var entity = await _context.Events
                .Where(x => x.Id == id && x.HostUUID == user.Id)
                .SingleOrDefaultAsync();

            if (entity == null) return false;

            entity.EventName = newItem.Name;
            entity.EventDescription = newItem.Description;
            entity.EventTime = newItem.EventTime;

            _context.Events.Update(entity);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<IEnumerable<EventItem>> GetIncompleteItemsAsync(ApplicationUser user)
        {
            return await _context.Events
                .Where(x => !x.IsDeleted && x.HostUUID == user.Id)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<EventItem>> GetUpcomingEventsAsync()
        {
            return await _context.Events
                .Where(x => !x.IsDeleted)
                .ToArrayAsync();
        }

        public async Task<bool> RemoveEventAsync(Guid id, ApplicationUser user)
        {
            var item = await _context.Events
                .Where(x => x.Id == id && x.HostUUID == user.Id)
                .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDeleted = true;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }
    }
}