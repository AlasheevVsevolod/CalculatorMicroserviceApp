using Calculator.AdditionService.Models;
using Calculator.Common.Messaging;
using MassTransit;
using MediatR;

namespace Calculator.AdditionService.Activities;

public class AdditionActivity(ISender mediator) : IActivity<AdditionActivityArguments, AdditionActivityLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<AdditionActivityArguments> context)
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

        return context.CompletedWithVariables(new AdditionActivityLog(result.CreatedId), new { Results = resultsStack });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<AdditionActivityLog> context)
    {
        var result = await mediator.Send(new RemoveOperationCommand(context.Log.CreatedId));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }
        return context.Compensated();
    }
}
