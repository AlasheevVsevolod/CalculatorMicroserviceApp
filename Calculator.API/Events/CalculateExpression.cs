namespace Calculator.API.Events;

public record CalculateExpression
{
    public List<string> ObjectsInPolishNotation { get; init; }
    public Guid OperationId { get; init; }
}
