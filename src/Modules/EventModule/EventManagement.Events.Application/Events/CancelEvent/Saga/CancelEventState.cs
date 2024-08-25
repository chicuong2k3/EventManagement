using MassTransit;

namespace EventManagement.Events.Application.Events.CancelEvent.Saga;

public sealed class CancelEventState : SagaStateMachineInstance, ISagaVersion
{

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public int Version { get; set; }
    public int CancellationCompletedStatus { get; set; }
}
