using MediatR;

namespace EventManagement.Common.Domain.DomainEvents
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}
