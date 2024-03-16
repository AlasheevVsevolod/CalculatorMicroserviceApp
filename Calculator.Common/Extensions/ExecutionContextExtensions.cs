using Calculator.Common.Messaging;
using Calculator.Common.Models;
using MassTransit;

namespace Calculator.Common.Extensions;

public static class ExecutionContextExtensions
{
    public static List<double> GetResultsVariable(this ExecuteContext<BinaryActivityArguments> context)
    {
        return ((List<object>)context.Message.Variables["Results"]).Select(Convert.ToDouble).ToList();
    }
    public static (double Operand1, double Operand2) GetBinaryOperands(
        this ExecuteContext<BinaryActivityArguments> context,
        List<double> results)
    {
        var message = context.Arguments;
        var operand1 = message.Operand1 ?? results.TakeLast();
        var operand2 = message.Operand2 ?? results.TakeLast();

        return (operand1, operand2);
    }

    public static ExecutionResult CompletedWithResultsVariable(
        this ExecuteContext<BinaryActivityArguments> context,
        Result result,
        List<double> results)
    {
        results.Add(result.Value);
        return context.CompletedWithVariables(new ActivityLog(result.CreatedId), new { Results = results });
    }
}
