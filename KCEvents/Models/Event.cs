using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCEvents.Models
{
        public class Event
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string ContactEmail { get; set; }
            public int CategoryId { get; set; }
            public int AddressId { get; set; }
            public string Picture { get; set; }
            public EventAddress Address { get; set; }
            public EventCategory Category { get; set; }

        public Event()
        {
        }
            public Event(string name, string description, string contactEmail)
            {
                Name = name;
                Description = description;
                ContactEmail = contactEmail;
      
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }