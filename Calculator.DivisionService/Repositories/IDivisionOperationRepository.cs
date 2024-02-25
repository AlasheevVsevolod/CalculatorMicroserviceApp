using Calculator.DivisionService.Models;

namespace Calculator.DivisionService.Repositories;

public interface IDivisionOperationRepository
{
    Task<Guid> CreateAsync(DivisionDocumentModel model);
    Task DeleteAsync(Guid id);
}
