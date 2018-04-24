using System.Collections.Generic;

namespace EventManager.Models
{
    public class EventViewModel
    {
        public IEnumerable<EventItem> Events { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
