using Calculator.Common.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiplicationService.Models;

namespace MultiplicationService.Repositories;

public class MultiplicationOperationRepository : IMultiplicationOperationRepository
{
    private readonly IMongoCollection<MultiplicationDocumentModel> _collection;

    public MultiplicationOperationRepository(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.ExpressionDatabaseName);
        _collection = database.GetCollection<MultiplicationDocumentModel>(mongoSettings.Value.ExpressionCollectionName);

    }

    public async Task<Guid> CreateAsync(MultiplicationDocumentModel model)
    {
        await _collection.InsertOneAsync(model);
        return Guid.Parse(model.Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id.ToString());
    }
}
