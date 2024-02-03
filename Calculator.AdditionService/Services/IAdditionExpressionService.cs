using Calculator.Common.Commands;

namespace Calculator.AdditionService.Services;

public interface IAdditionExpressionService
{
    Task<double> CalculateExpression(AdditionCommand command);
}
