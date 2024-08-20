namespace EventManagement.Ticketing.Domain.Events
{
    public sealed class EventEntity : Entity
    {
        private EventEntity()
        {
        }

        public Guid Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string Location { get; private set; }

        public DateTime StartsAt { get; private set; }

        public DateTime? EndsAt { get; private set; }

        public bool Cancelled { get; private set; }

        public static EventEntity Create(
            Guid id,
            string title,
            string description,
            string location,
            DateTime startsAt,
            DateTime? endsAt)
        {
            var eventEntity = new EventEntity
            {
                Id = id,
                Title = title,
                Description = description,
                Location = location,
                StartsAt = startsAt,
                EndsAt = endsAt
            };

            return eventEntity;
        }

        public Result Reschedule(DateTime startsAt, DateTime? endsAt)
        {
            if (startsAt < DateTime.Now)
            {
                return Result.Failure(EventErrors.StartDateInPast);
            }

            StartsAt = startsAt;
            EndsAt = endsAt;

            Raise(new EventRescheduledDomainEvent(Id, StartsAt, EndsAt));

            return Result.Success();
        }

        public void Cancel()
        {
            if (Cancelled)
            {
                return;
            }

            Cancelled = true;

            Raise(new EventCancelledDomainEvent(Id));
        }

        public void PaymentsRefunded()
        {
            Raise(new EventPaymentsRefundedDomainEvent(Id));
        }

        public void TicketsArchived()
        {
            Raise(new EventTicketsArchivedDomainEvent(Id));
        }
    }
}
