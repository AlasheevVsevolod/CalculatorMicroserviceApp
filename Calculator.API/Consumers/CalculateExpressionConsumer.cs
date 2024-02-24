using Calculator.AdditionService.Activities;
using Calculator.API.Enums;
using Calculator.API.Events;
using Calculator.API.Extensions;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace Calculator.API.Consumers;

public class CalculateExpressionConsumer(IEndpointNameFormatter endpointNameFormatter) : IConsumer<CalculateExpression>
{
    public async Task Consume(ConsumeContext<CalculateExpression> context)
    {
        try
        {
            var routingSlip = CreateRoutingSlip(context);
            await context.Execute(routingSlip);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private RoutingSlip CreateRoutingSlip(ConsumeContext<CalculateExpression> context)
    {
        var builder = new RoutingSlipBuilder(Guid.NewGuid());
        var items = ParseObjects(context.Message.ObjectsInPolishNotation);
        builder.AddVariable("Result", items.First(x => x is double));

        var resultStack = new Stack<double>();
        foreach (var operand in items)
        {
            if (operand is double)
            {
                resultStack.Push((double)operand);
                continue;
            }

            resultStack.TryPop(out var op1);
            switch ((Operations)operand)
            {
                case Operations.Add:
                    builder.AddActivity(
                        "AdditionActivity",
                        new Uri($"exchange:{endpointNameFormatter.ExecuteActivity<AdditionActivity, AdditionArguments>()}"),
                        new { Operand1 = op1 });
                    break;
                case Operations.Subtract:
                    // resultStack.Push(op1 - op2);
                    break;
                case Operations.Multiply:
                    // resultStack.Push(op1 * op2);
                    break;
                case Operations.Divide:
                    // resultStack.Push(op1 / op2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.ActivityFaulted, RoutingSlipEventContents.All,
            x => x.Send<CalculationFailed>(new { context.Message.OperationId }));

        builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.Completed, RoutingSlipEventContents.Variables,
            x => x.Send<CalculationCompleted>(new { context.Message.OperationId }));

        var routingSlip = builder.Build();
        return routingSlip;
    }

    private List<object> ParseObjects(List<string> objectsFromMessage)
    {
        var result = new List<object>();
        foreach (var item in objectsFromMessage)
        {
            if (item.TryGetOperation(out var operation))
            {
                result.Add(operation);
            }
            else
            {
                result.Add(double.Parse(item));
            }
        }

        return result;
    }
}
