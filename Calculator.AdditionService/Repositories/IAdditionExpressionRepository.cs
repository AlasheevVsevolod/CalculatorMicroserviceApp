using Calculator.AdditionService.Models;

namespace Calculator.AdditionService.Repositories;

public interface IAdditionExpressionRepository
{
    Task<Guid> CreateAsync(AdditionExpressionModel model);
    Task DeleteAsync(Guid id);
}
