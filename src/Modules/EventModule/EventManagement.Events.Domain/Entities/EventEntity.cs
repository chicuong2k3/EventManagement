using EventManagement.Events.Domain.DomainEvents.Events;
using EventManagement.Events.Domain.Entities.Enums;

namespace EventManagement.Events.Domain.Entities
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
        public Guid CategoryId { get; private set; }
        public DateTime StartsAt { get; private set; }
        public DateTime? EndsAt { get; private set; }
        public EventStatus Status { get; private set; }

        public static Result<EventEntity> Create(
            string title, 
            string description, 
            string location, 
            Guid categoryId,
            DateTime startsAt,
            DateTime? endsAt)
        {

            if (endsAt.HasValue && endsAt < startsAt)
            {
                return Result.Failure<EventEntity>(EventErrors.EndDatePrecedesStartDate);
            }

            var eventEntity = new EventEntity()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Location = location,
                CategoryId = categoryId,
                StartsAt = startsAt,
                EndsAt = endsAt,
                Status = EventStatus.Draft
            };

            eventEntity.Raise(new EventCreated(eventEntity.Id));

            return eventEntity;
        }

        public Result Publish()
        {
            if (Status != EventStatus.Draft)
            {
                return Result.Failure(EventErrors.NotDraft);
            }

            Status = EventStatus.Published;

            Raise(new EventPublished(Id));

            return Result.Success();
        }

        public Result Reschedule(DateTime startsAt, DateTime? endsAt)
        {

            if (startsAt > DateTime.Now)
            {
                return Result.Failure(EventErrors.StartDateInPast);
            }

            StartsAt = startsAt;
            EndsAt = endsAt;

            Raise(new EventRescheduled(Id, StartsAt, EndsAt));

            return Result.Success();
        }

        public Result Cancel()
        {
            if (Status == EventStatus.Cancelled)
            {
                return Result.Failure(EventErrors.AlreadyCancelled);
            }

            if (StartsAt < DateTime.Now)
            {
                return Result.Failure(EventErrors.AlreadyStarted);
            }

            Status = EventStatus.Cancelled;

            Raise(new EventCancelled(Id));

            return Result.Success();
        }
    }

}
