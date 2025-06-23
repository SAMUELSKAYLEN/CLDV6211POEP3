using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ST10443307CLVDProjectPOEPart3.Models
{
    public class Event
    {
        public int EVENTID { get; set; }

        [Required]

        public required string EventName { get; set; }

        [Required]

        public DateTime EventDate { get; set; }
        public string? Descript { get; set; }
        public int VENUEID { get; set; }
        public Venue? Venue { get; set; } //Each Event has one Venue

        public int? EventTypeID { get; set; } //foreign key column
        public EventType? EventType { get; set; } //navigation property for EventType object
        public List<Booking> Bookings { get; set; } = new();
    }
}
