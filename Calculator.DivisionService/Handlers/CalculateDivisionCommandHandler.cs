using Calculator.Common.Abstractions;
using Calculator.Common.Messaging;
using Calculator.DivisionService.Models;
using Calculator.DivisionService.Repositories;

namespace Calculator.DivisionService.Handlers;

public class CalculateDivisionCommandHandler(IDivisionOperationRepository operationRepository)
    : ICommandHandler<CalculateOperationCommand>
{
    public async Task<Result> Handle(CalculateOperationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var operationResult = command.Operand1 / command.Operand2;
            var newExpression = new DivisionDocumentModel
            {
                Expression = command.Operand1 + " / " + command.Operand2,
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
