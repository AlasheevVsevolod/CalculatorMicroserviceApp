using Calculator.Common.Configurations;
using Calculator.DivisionService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Calculator.DivisionService.Repositories;

public class DivisionOperationRepository : IDivisionOperationRepository
{
    private readonly IMongoCollection<DivisionDocumentModel> _collection;

    public DivisionOperationRepository(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.ExpressionDatabaseName);
        _collection = database.GetCollection<DivisionDocumentModel>(mongoSettings.Value.ExpressionCollectionName);

    }

    public async Task<Guid> CreateAsync(DivisionDocumentModel model)
    {
        await _collection.InsertOneAsync(model);
        return Guid.Parse(model.Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id.ToString());
    }
}
