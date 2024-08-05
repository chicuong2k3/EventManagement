﻿namespace EventManagement.Common.Domain.DomainEvents
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}