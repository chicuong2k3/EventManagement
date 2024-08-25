namespace EventManagement.Attendance.Domain.Events
{
    public sealed class EventStatistics
    {
        public Guid EventId { get; private set; }
        public string Title { get; private set; }

        public string Description { get; private set; }

        public string Location { get; private set; }

        public DateTime StartsAt { get; private set; }

        public DateTime? EndsAt { get; private set; }
        public int TicketsSold { get; private set; }
        public int AttendeesCheckedIn { get; private set; }
        public List<string> DuplicateCheckInTickets { get; private set; }
        public List<string> InvalidCheckInTickets { get; private set; }

        private EventStatistics()
        {
            
        }
        public EventStatistics Create(
            Guid eventId, 
            string title, 
            string description, 
            string location, 
            DateTime startsAt, 
            DateTime? endsAt, 
            int ticketsSold, 
            int attendeesCheckedIn, 
            List<string> duplicateCheckInTickets, 
            List<string> invalidCheckInTickets)
        {
            var eventStatistics = new EventStatistics()
            {
                EventId = eventId,
                Title = title,
                Description = description,
                Location = location,
                StartsAt = startsAt,
                EndsAt = endsAt,
                TicketsSold = ticketsSold,
                AttendeesCheckedIn = attendeesCheckedIn,
                DuplicateCheckInTickets = duplicateCheckInTickets,
                InvalidCheckInTickets = invalidCheckInTickets
            };

            return eventStatistics;
        }
    }
}
