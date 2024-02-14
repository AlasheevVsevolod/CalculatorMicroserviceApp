using Calculator.AdditionService.Models;
using MassTransit;
using MediatR;

namespace Calculator.AdditionService.Activities;

public class AdditionActivity(ISender mediator) : IActivity<AdditionArguments, AdditionLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<AdditionArguments> context)
    {
        var message = context.Arguments;
        var result = await mediator.Send(new CalculateAdditionCommand(message.Operand1, Convert.ToDouble(context.Message.Variables["Result"])));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }

        return context.CompletedWithVariables(new AdditionLog(result.CreatedId), new { Result = result.Value });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<AdditionLog> context)
    {
        var result = await mediator.Send(new RemoveAdditionCommand(context.Log.CreatedId));
        if (result.IsFailed)
        {
            throw new Exception(string.Join(",\n", result.Errors));
        }
        return context.Compensated();
    }
}
