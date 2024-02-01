using Calculator.AdditionService.Models;
using Calculator.AdditionService.Repositories;
using Calculator.Common.Commands;

namespace Calculator.AdditionService.Services;

public class AdditionExpressionService(IAdditionExpressionRepository repository) : IAdditionExpressionService
{
    public async Task<double> CalculateExpression(AdditionCommand command)
    {
        var result = command.op1 + command.op2;
        var newExpression = new AdditionExpressionModel
        {
            Expression = command.op1 + "+" + command.op2,
            Result = result
        };
        await repository.CreateAsync(newExpression);

        return result;
    }
}
