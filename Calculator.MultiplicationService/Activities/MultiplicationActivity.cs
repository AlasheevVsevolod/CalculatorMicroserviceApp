using Calculator.Common.Messaging;
using MassTransit;
using MediatR;
using MultiplicationService.Models;

namespace MultiplicationService.Activities;

public class MultiplicationActivity(ISender mediator) : IActivity<MultiplicationActivityArguments, MultiplicationActivityLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<MultiplicationActivityArguments> context)
    {
        var resultsStack = new Stack<double>(((List<object>)context.Message.Variables["Results"]).Select(Convert.ToDouble).ToList());
        var message = context.Arguments;
        var operand1 = message.Operand1 ?? resultsStack.Pop();
        var operand2 = message.Operand2 ?? resultsStack.Pop();
        var result = await mediator.Send(new CalculateOperationCommand(operand1, operand2));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }

        resultsStack.Push(result.Value);

        return context.CompletedWithVariables(new MultiplicationActivityLog(result.CreatedId), new { Results = resultsStack });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<MultiplicationActivityLog> context)
    {
        var result = await mediator.Send(new RemoveOperationCommand(context.Log.CreatedId));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }
        return context.Compensated();
    }
}
