using Calculator.Common.Extensions;
using Calculator.Common.Messaging;
using Calculator.Common.Models;
using MassTransit;
using MediatR;

namespace Calculator.AdditionService.Activities;

public class AdditionActivity(ISender mediator) : IActivity<BinaryActivityArguments, ActivityLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<BinaryActivityArguments> context)
    {
        var results = context.GetResultsVariable();
        var (operand1, operand2) = context.GetBinaryOperands(results);
        var result = await mediator.Send(new CalculateOperationCommand(operand1, operand2));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }

        return context.CompletedWithResultsVariable(result, results);
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ActivityLog> context)
    {
        var result = await mediator.Send(new RemoveOperationCommand(context.Log.CreatedId));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }
        return context.Compensated();
    }
}
