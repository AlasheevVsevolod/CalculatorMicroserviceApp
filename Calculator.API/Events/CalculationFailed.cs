using MassTransit;
using MassTransit.Courier.Contracts;

namespace Calculator.API.Events;

public record CalculationFailed : RoutingSlipActivityFaulted
{
    public Guid OperationId { get; init; }
    public Guid TrackingNumber { get; init; }
    public Guid ExecutionId { get; init; }
    public DateTime Timestamp { get; init; }
    public TimeSpan Duration { get; init; }
    public string ActivityName { get; init; }
    public HostInfo Host { get; init; }
    public ExceptionInfo ExceptionInfo { get; init; }
    public IDictionary<string, object> Arguments { get; init; }
    public IDictionary<string, object> Variables { get; init; }
}
