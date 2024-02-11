using Calculator.AdditionService.Models;
using Calculator.AdditionService.Repositories;
using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;

namespace Calculator.AdditionService.Handlers;

public class CalculateAdditionCommandHandler(IAdditionExpressionRepository additionExpressionRepository)
    : ICommandHandler<CalculateAdditionCommand>
{
    public async Task<Result> Handle(CalculateAdditionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var operationResult = command.Operand1 + command.Operand2;
            var newExpression = new AdditionExpressionModel
            {
                Expression = command.Operand1 + " + " + command.Operand2,
                Result = operationResult
            };

            var createdId = await additionExpressionRepository.CreateAsync(newExpression);
            return new Result { Value = operationResult, CreatedId = createdId};
        }
        catch (Exception e)
        {
            return new Result { Errors = [e.ToString()] };
        }
    }
}
