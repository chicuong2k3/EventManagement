

namespace EventManagement.Events.Entities
{
    public class EventModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public EventStatus Status { get; set; }
        public double TicketPrice { get; set; }
    }

    public enum EventStatus
    {
        Draft = 0,
        Published = 1,
        Completed = 2,
        Cancelled = 3
    }
}
