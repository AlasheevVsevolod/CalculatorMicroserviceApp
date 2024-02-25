using Calculator.Common.Messaging;
using MassTransit;
using MediatR;
using SubtractionService.Models;

namespace SubtractionService.Activities;

public class SubtractionActivity(ISender mediator) : IActivity<SubtractionActivityArguments, SubtractionActivityLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<SubtractionActivityArguments> context)
    {
        var message = context.Arguments;
        var operand1 = message.Operand1;
        var operand2 = Convert.ToDouble(context.Message.Variables["Result"]);
        var result = await mediator.Send(new CalculateOperationCommand(operand1, operand2));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }

        return context.CompletedWithVariables(new SubtractionActivityLog(result.CreatedId), new { Result = result.Value });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<SubtractionActivityLog> context)
    {
        var result = await mediator.Send(new RemoveOperationCommand(context.Log.CreatedId));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }
        return context.Compensated();
    }
}
