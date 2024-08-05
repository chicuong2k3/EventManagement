namespace EventManagement.Events.Domain.Errors
{
    public static class EventErrors
    {
        public static Error NotFound(Guid eventId) => Error.NotFound(
            "EventEntity.NotFound",
            $"The event with the identifier {eventId} was not found");

        public static readonly Error StartDateInPast = Error.Problem(
            "EventEntity.StartDateInPast",
            "The event start date is in the past");

        public static readonly Error EndDatePrecedesStartDate = Error.Problem(
            "EventEntity.EndDatePrecedesStartDate",
            "The event end date precedes the start date");

        public static readonly Error NoTicketsFound = Error.Problem(
            "EventEntity.NoTicketsFound",
            "The event does not have any tickets defined");

        public static readonly Error NotDraft = Error.Problem(
            "EventEntities.NotDraft",
            "The event is not in draft status");

        public static readonly Error AlreadyCancelled = Error.Problem(
            "EventEntity.AlreadyCancelled",
            "The event was already cancelled");

        public static readonly Error AlreadyStarted = Error.Problem(
            "EventEntity.AlreadyStarted",
            "The event has already started");
    }
}
