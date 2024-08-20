using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Users.IntegrationEvents
{
    public sealed class UserUpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public UserUpdatedIntegrationEvent()
        {

        }
        public UserUpdatedIntegrationEvent(
            Guid eventId,
            DateTime occurredOn,
            Guid userId, 
            string firstName, 
            string lastName)
            : base(eventId, occurredOn)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
