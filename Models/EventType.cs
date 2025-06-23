using System.ComponentModel.DataAnnotations;

namespace ST10443307CLVDProjectPOEPart3.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeID { get; set; }
        public string EventTypeName { get; set; }
    }
}
