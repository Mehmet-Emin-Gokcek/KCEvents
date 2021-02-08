using KCEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KCEvents.ViewModels
{
    public class EventDetailViewModel
    {

        public int EventId { get; set; }
        public string TagText { get; set; }
        public string Name { get; set; }
        public string Description {get; set;}
        public string ContactEmail{ get; set; }
        public string CategoryName{ get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string State { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private const double homeLatitude = 39.0942823;

        private const double homeLongitude = -94.5906067;
        public double HomeLatitude { get { return homeLatitude; } }
        public double HomeLongitude { get { return homeLongitude;  } }
        public string Picture { get; set; }

        public EventDetailViewModel(Event theEvent, List<EventTag> eventTags)
        {
            EventId = theEvent.Id;
            Name = theEvent.Name;
            Description = theEvent.Description;
            ContactEmail = theEvent.ContactEmail;
            CategoryName = theEvent.Category.Name;
            Street = theEvent.Address.Street;
            City = theEvent.Address.City;
            Zipcode = theEvent.Address.Zipcode;
            State = theEvent.Address.State;
            Latitude = theEvent.Address.Latitude;
            Longitude = theEvent.Address.Longitude;
            Picture = theEvent.Picture;

            TagText = "";
            for (var i = 0; i < eventTags.Count; i++)
            {
                TagText += ("#" + eventTags[i].Tag.Name);
                if (i < eventTags.Count - 1)
                {
                    TagText += ", ";
                }
            }

        }
    }
}
