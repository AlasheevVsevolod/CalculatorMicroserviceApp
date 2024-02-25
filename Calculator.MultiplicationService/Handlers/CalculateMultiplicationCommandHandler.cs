using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;
using MultiplicationService.Models;
using MultiplicationService.Repositories;

namespace MultiplicationService.Handlers;

public class CalculateMultiplicationCommandHandler(IMultiplicationOperationRepository operationRepository)
    : ICommandHandler<CalculateOperationCommand>
{
    public async Task<Result> Handle(CalculateOperationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var operationResult = command.Operand1 * command.Operand2;
            var newExpression = new MultiplicationDocumentModel
            {
                Expression = command.Operand1 + " * " + command.Operand2,
                Result = operationResult
            };

            var createdId = await operationRepository.CreateAsync(newExpression);
            return new Result { Value = operationResult, CreatedId = createdId};
        }
        catch (Exception e)
        {
            return new Result { Errors = [e.ToString()] };
        }
    }
}
