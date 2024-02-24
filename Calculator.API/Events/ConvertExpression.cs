namespace Calculator.API.Events;

public record ConvertExpression
{
    public string Expression { get; init; }
    public Guid OperationId { get; init; }
}
