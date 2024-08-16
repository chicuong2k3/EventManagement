namespace EventManagement.Ticketing.Domain.Errors
{
    public static class EventErrors
    {
        public static Error NotFound(Guid eventId) =>
            Error.NotFound("Event.NotFound", $"The event with the identifier {eventId} was not found");

        public static readonly Error StartDateInPast = Error.Problem(
            "Event.StartDateInPast",
            "The event start date is in the past");
    }
}
