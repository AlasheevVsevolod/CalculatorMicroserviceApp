using Calculator.Common.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SubtractionService.Models;

namespace SubtractionService.Repositories;

public class SubtractionOperationRepository : ISubtractionOperationRepository
{
    private readonly IMongoCollection<SubtractionDocumentModel> _collection;

    public SubtractionOperationRepository(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.ExpressionDatabaseName);
        _collection = database.GetCollection<SubtractionDocumentModel>(mongoSettings.Value.ExpressionCollectionName);

    }

    public async Task<Guid> CreateAsync(SubtractionDocumentModel model)
    {
        await _collection.InsertOneAsync(model);
        return Guid.Parse(model.Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id.ToString());
    }
}
