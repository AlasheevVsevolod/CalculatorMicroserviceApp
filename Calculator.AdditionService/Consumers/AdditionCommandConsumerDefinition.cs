using Calculator.Common.Constants;
using MassTransit;

namespace Calculator.AdditionService.Consumers;

public class AdditionCommandConsumerDefinition : ConsumerDefinition<AdditionCommandConsumer>
{
    public AdditionCommandConsumerDefinition()
    {
        Endpoint(x => x.Name = GlobalEndpointAddress.CalculatorAdditionCommandQueue);
    }
}
