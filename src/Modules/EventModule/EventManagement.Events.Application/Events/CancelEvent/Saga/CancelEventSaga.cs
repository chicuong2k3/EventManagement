using EventManagement.Ticketing.IntegrationEvents;
using MassTransit;

namespace EventManagement.Events.Application.Events.CancelEvent.Saga;
public sealed class CancelEventSaga : MassTransitStateMachine<CancelEventState>
{
    public State CancellationStarted { get; private set; }
    public State PaymentsRefunded { get; private set; }
    public State TicketsArchived { get; private set; }
    
    public Event<EventCancelledIntegrationEvent> EventCancelled { get; private set; }
    public Event<EventPaymentsRefundedIntegrationEvent> EventPaymentsRefunded { get; private set; }
    public Event<EventTicketsArchivedIntegrationEvent> EventTicketsArchived { get; private set; }
    public Event EventCancellationCompleted { get; private set; }
    public CancelEventSaga()
    {
        Event(() => EventCancelled, x => x.CorrelateById(m => m.Message.EventId));
        Event(() => EventPaymentsRefunded, x => x.CorrelateById(m => m.Message.EventId));
        Event(() => EventTicketsArchived, x => x.CorrelateById(m => m.Message.EventId));

        InstanceState(x => x.CurrentState);

        Initially(
            When(EventCancelled)
                .Publish(context =>
                {
                    return new EventCancellationStartedIntegrationEvent(
                        context.Message.Id,
                        context.Message.OccurredOn,
                        context.Message.EventId);
                })
                .TransitionTo(CancellationStarted)
        );

        During(CancellationStarted,
            When(EventPaymentsRefunded)
            .TransitionTo(PaymentsRefunded),
            When(EventTicketsArchived)
            .TransitionTo(TicketsArchived)
        );

        During(PaymentsRefunded,
            When(EventTicketsArchived)
            .TransitionTo(TicketsArchived)
        );

        During(TicketsArchived,
            When(EventPaymentsRefunded)
            .TransitionTo(PaymentsRefunded)
        );

        CompositeEvent(
            () => EventCancellationCompleted,
            state => state.CancellationCompletedStatus,
            EventPaymentsRefunded, EventTicketsArchived);

        DuringAny(
            When(EventCancellationCompleted)
            .Publish(context => 
                new EventCancellationCompletedIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                context.Saga.CorrelationId)
            )
            .Finalize()
        );
    }
}
