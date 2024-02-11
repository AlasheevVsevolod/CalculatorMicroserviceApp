using Calculator.AdditionService.Models;
using Calculator.Common.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Calculator.AdditionService.Repositories;

public class AdditionExpressionRepository : IAdditionExpressionRepository
{
    private readonly IMongoCollection<AdditionExpressionModel> _collection;

    public AdditionExpressionRepository(IMongoClient mongoClient, IOptions<MongoSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _collection = database.GetCollection<AdditionExpressionModel>(mongoSettings.Value.CollectionName);

    }
    public async Task<Guid> CreateAsync(AdditionExpressionModel model)
    {
        await _collection.InsertOneAsync(model);
        return Guid.Parse(model.Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id.ToString());
    }
}
