using Calculator.AdditionService.Models;

namespace Calculator.AdditionService.Repositories;

public interface IAdditionExpressionRepository
{
    Task CreateAsync(AdditionExpressionModel model);
}
