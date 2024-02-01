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
    public async Task CreateAsync(AdditionExpressionModel model)
    {
        await _collection.InsertOneAsync(model);
    }
}
