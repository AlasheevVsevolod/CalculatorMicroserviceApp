using Calculator.AdditionService.Models;

namespace Calculator.AdditionService.Repositories;

public interface IAdditionOperationRepository
{
    Task<Guid> CreateAsync(AdditionDocumentModel model);
    Task DeleteAsync(Guid id);
}
