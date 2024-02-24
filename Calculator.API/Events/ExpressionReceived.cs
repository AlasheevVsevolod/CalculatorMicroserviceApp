namespace Calculator.API.Events;

public record ExpressionReceived
{
    public string Expression { get; init; }
    public Guid OperationId { get; init; }
};
