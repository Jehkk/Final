using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models
{
    public class EventItem
    {
        public Guid Id { get; set; }

        public string HostUUID { get; set; }

        public string EventName { get; set; }

        public string EventDescription { get; set; }

        public DateTimeOffset? EventTime { get; set; }

        public DateTimeOffset? EventCreated { get; set; }

        public bool IsDeleted { get; set; }
    }
}
