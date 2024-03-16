namespace Calculator.API.Events;

public record CalculationStatusRequested
{
    public Guid OperationId { get; init; }
}
