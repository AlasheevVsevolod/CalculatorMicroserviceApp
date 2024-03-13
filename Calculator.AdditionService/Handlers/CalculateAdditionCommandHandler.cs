using Calculator.AdditionService.Models;
using Calculator.AdditionService.Repositories;
using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;

namespace Calculator.AdditionService.Handlers;

public class CalculateAdditionCommandHandler(IAdditionOperationRepository additionOperationRepository)
    : ICommandHandler<CalculateOperationCommand>
{
    public async Task<Result> Handle(CalculateOperationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var operationResult = command.Operand1 + command.Operand2;
            var newExpression = new AdditionDocumentModel
            {
                Expression = command.Operand1 + " + " + command.Operand2,
                Result = operationResult
            };

            var createdId = await additionOperationRepository.CreateAsync(newExpression);
            return new Result { Value = operationResult, CreatedId = createdId};
        }
        catch (Exception e)
        {
            return new Result { Errors = [e.ToString()] };
        }
    }
}
