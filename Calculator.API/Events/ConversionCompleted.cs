namespace Calculator.API.Events;

public record ConversionCompleted
{
    public List<string> ObjectsInPolishNotation { get; init; }
    public Guid OperationId { get; init; }
}
