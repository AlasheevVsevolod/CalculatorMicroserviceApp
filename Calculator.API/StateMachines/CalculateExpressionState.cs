using MassTransit;

namespace Calculator.API.StateMachines;

public class CalculateExpressionState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public string Expression { get; set; } = string.Empty;
    public double Result { get; set; }
    public string CurrentState { get; set; } = "Initial";
    public string Reason { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTime UpdatedOn = DateTime.UtcNow;
    public DateTime StartedOn { get; set; }
    public DateTime FinishedOn { get; set; }
    public TimeSpan Duration { get; set; }
}
