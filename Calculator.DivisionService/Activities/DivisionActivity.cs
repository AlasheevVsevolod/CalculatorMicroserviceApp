using Calculator.Common.Messaging;
using Calculator.DivisionService.Models;
using MassTransit;
using MediatR;

namespace Calculator.DivisionService.Activities;

public class DivisionActivity(ISender mediator) : IActivity<DivisionActivityArguments, DivisionActivityLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<DivisionActivityArguments> context)
    {
        var message = context.Arguments;
        var operand1 = message.Operand1;
        var operand2 = Convert.ToDouble(context.Message.Variables["Result"]);
        var result = await mediator.Send(new CalculateOperationCommand(operand1, operand2));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }

        return context.CompletedWithVariables(new DivisionActivityLog(result.CreatedId), new { Result = result.Value });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<DivisionActivityLog> context)
    {
        var result = await mediator.Send(new RemoveOperationCommand(context.Log.CreatedId));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }
        return context.Compensated();
    }
}
