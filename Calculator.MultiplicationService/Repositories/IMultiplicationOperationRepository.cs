using MultiplicationService.Models;

namespace MultiplicationService.Repositories;

public interface IMultiplicationOperationRepository
{
    Task<Guid> CreateAsync(MultiplicationDocumentModel model);
    Task DeleteAsync(Guid id);
}
