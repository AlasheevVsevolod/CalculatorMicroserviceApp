namespace Calculator.API.Models;

public record CalculationStatus
{
    public string OperationId { get; init; }
    public string Expression { get; init; }
    public double Result { get; init; }
    public string CurrentState { get; init; }
    public string Reason { get; init; }
    public DateTime UpdatedOn { get; init; }
    public DateTime StartedOn { get; init; }
    public DateTime FinishedOn { get; init; }
    public TimeSpan Duration { get; init; }
}
