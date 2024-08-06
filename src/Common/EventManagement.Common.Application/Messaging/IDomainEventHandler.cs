using EventManagement.Common.Domain.DomainEvents;
using MediatR;

namespace EventManagement.Common.Application.Messaging;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{ }