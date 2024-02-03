using Calculator.AdditionService.Services;
using Calculator.Common.Commands;
using MassTransit;

namespace Calculator.AdditionService.Consumers;

public class AdditionCommandConsumer(IAdditionExpressionService service) : IConsumer<AdditionCommand>
{
    public async Task Consume(ConsumeContext<AdditionCommand> context)
    {
        var result = await service.CalculateExpression(context.Message);
    }
}
