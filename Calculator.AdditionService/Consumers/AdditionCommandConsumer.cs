using Calculator.AdditionService.Models;
using Calculator.Common.RabbitMessages;
using MassTransit;
using MediatR;

namespace Calculator.AdditionService.Consumers;

public class AdditionCommandConsumer(ISender mediator) : IConsumer<AdditionMessage>
{
    public async Task Consume(ConsumeContext<AdditionMessage> context)
    {
        var message = context.Message;
        var result = await mediator.Send(new CalculateAdditionCommand(message.Operand1, message.Operand2));
    }
}
