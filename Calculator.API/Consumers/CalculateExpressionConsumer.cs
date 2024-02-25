using Calculator.AdditionService.Activities;
using Calculator.AdditionService.Models;
using Calculator.API.Enums;
using Calculator.API.Events;
using Calculator.API.Extensions;
using Calculator.DivisionService.Activities;
using Calculator.DivisionService.Models;
using MassTransit;
using MassTransit.Courier.Contracts;
using MassTransit.Events;
using MultiplicationService.Activities;
using MultiplicationService.Models;
using SubtractionService.Activities;
using SubtractionService.Models;

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
            var errorMessage = new CalculationFailed{ ExceptionInfo = new FaultExceptionInfo(e), OperationId = context.Message.OperationId};
            await context.Publish(errorMessage);
        }
    }

    private RoutingSlip CreateRoutingSlip(ConsumeContext<CalculateExpression> context)
    {
        var builder = new RoutingSlipBuilder(Guid.NewGuid());
        var items = ParseObjects(context.Message.ObjectsInPolishNotation);
        var initialValueAdded = false;

        var resultStack = new Stack<double>();
        foreach (var operand in items)
        {
            if (operand is double)
            {
                resultStack.Push((double)operand);
                continue;
            }

            if (!initialValueAdded)
            {
                resultStack.TryPop(out var op2);
                builder.AddVariable("Result", op2);
                initialValueAdded = true;
            }

            resultStack.TryPop(out var op1);
            switch ((Operations)operand)
            {
                case Operations.Add:
                    builder.AddActivity(
                        "AdditionActivity",
                        new Uri($"exchange:{endpointNameFormatter.ExecuteActivity<AdditionActivity, AdditionActivityArguments>()}"),
                        new { Operand1 = op1 });
                    break;
                case Operations.Subtract:
                    builder.AddActivity(
                        "SubtractionActivity",
                        new Uri($"exchange:{endpointNameFormatter.ExecuteActivity<SubtractionActivity, SubtractionActivityArguments>()}"),
                        new { Operand1 = op1 });
                    break;
                case Operations.Multiply:
                    builder.AddActivity(
                        "MultiplicationActivity",
                        new Uri($"exchange:{endpointNameFormatter.ExecuteActivity<MultiplicationActivity, MultiplicationActivityArguments>()}"),
                        new { Operand1 = op1 });
                    break;
                case Operations.Divide:
                    builder.AddActivity(
                        "DivisionActivity",
                        new Uri($"exchange:{endpointNameFormatter.ExecuteActivity<DivisionActivity, DivisionActivityArguments>()}"),
                        new { Operand1 = op1 });
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
