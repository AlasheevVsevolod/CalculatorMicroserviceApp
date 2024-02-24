namespace Calculator.API.Events;

public record ConversionFailed
{
    public string ErrorMessage { get; init; }
    public Guid OperationId { get; init; }
}
