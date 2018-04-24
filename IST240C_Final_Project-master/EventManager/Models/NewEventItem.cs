using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models
{
    public class NewEventItem
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? EventTime { get; set; }
    }
}
