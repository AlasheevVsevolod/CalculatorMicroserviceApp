using SubtractionService.Models;

namespace SubtractionService.Repositories;

public interface ISubtractionOperationRepository
{
    Task<Guid> CreateAsync(SubtractionDocumentModel model);
    Task DeleteAsync(Guid id);
}
