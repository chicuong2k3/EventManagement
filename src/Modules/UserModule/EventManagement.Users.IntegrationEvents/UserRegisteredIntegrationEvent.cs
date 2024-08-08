using EventManagement.Common.Application.EventBuses;

namespace EventManagement.Users.IntegrationEvents
{
    public sealed class UserRegisteredIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        // this constructor ensures that the deserialization works 
        public UserRegisteredIntegrationEvent()
        {

        }
        public UserRegisteredIntegrationEvent(
            Guid eventId,
            DateTime occurredOn,
            Guid userId, 
            string email, 
            string firstName, 
            string lastName)
            : base(eventId, occurredOn)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
