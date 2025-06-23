namespace ST10443307CLVDProjectPOEPart3.Models
{
    public class Booking
    {
        

        public int BOOKINGID { get; set; }
        public int EVENTID { get; set; }
        public int VENUEID { get; set; }
        public Event? Event { get; set; }
        public Venue? Venue { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
