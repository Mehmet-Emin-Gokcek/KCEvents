﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KCEvents.Models
{
    public class EventAddress
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        public string Zipcode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        private const double homeLatitude = 39.0942823;

        private const double homeLongitude = -94.5906067;
        public double HomeLatitude { get { return homeLatitude; } }
        public double HomeLongitude { get { return homeLongitude; } }
        public EventAddress() { }

    }


}
