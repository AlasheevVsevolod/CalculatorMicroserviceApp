using MassTransit.Courier.Contracts;

namespace Calculator.API.Events;

public class CalculationCompleted : RoutingSlipCompleted
{
    public Guid OperationId { get; init; }
    public Guid TrackingNumber { get; init; }
    public DateTime Timestamp { get; init; }
    public TimeSpan Duration { get; init; }
    public IDictionary<string, object> Variables { get; init; }
}
