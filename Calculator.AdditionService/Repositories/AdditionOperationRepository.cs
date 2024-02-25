using Calculator.AdditionService.Models;
using Calculator.Common.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Calculator.AdditionService.Repositories;

public class AdditionOperationRepository : IAdditionOperationRepository
{
    private readonly IMongoCollection<AdditionDocumentModel> _collection;

    public AdditionOperationRepository(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.ExpressionDatabaseName);
        _collection = database.GetCollection<AdditionDocumentModel>(mongoSettings.Value.ExpressionCollectionName);

    }

    public async Task<Guid> CreateAsync(AdditionDocumentModel model)
    {
        await _collection.InsertOneAsync(model);
        return Guid.Parse(model.Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id.ToString());
    }
}
