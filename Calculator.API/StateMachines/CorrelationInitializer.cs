using System.Runtime.CompilerServices;
using Calculator.API.Events;
using MassTransit;

namespace Calculator.API.StateMachines;

public class CorrelationInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        MessageCorrelation.UseCorrelationId<CalculateExpression>(x => x.OperationId);
        MessageCorrelation.UseCorrelationId<CalculationCompleted>(x => x.OperationId);
        MessageCorrelation.UseCorrelationId<CalculationFailed>(x => x.OperationId);
        MessageCorrelation.UseCorrelationId<ConversionCompleted>(x => x.OperationId);
        MessageCorrelation.UseCorrelationId<ConversionFailed>(x => x.OperationId);
        MessageCorrelation.UseCorrelationId<ConvertExpression>(x => x.OperationId);
        MessageCorrelation.UseCorrelationId<ExpressionReceived>(x => x.OperationId);
    }
}
