﻿using EventManagement.Events.Domain.Events;

namespace EventManagement.Events.Infrastructure.Events
{
    internal sealed class EventRepository(EventsDbContext dbContext) : IEventRepository
    {
        public async Task<EventEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Events.FindAsync(id, cancellationToken);
        }

        public void Insert(EventEntity eventEntity)
        {
            dbContext.Events.Add(eventEntity);
        }
    }
}