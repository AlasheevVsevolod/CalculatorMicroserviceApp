using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Calculator.AdditionService.Models;

public class AdditionExpressionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Expression { get; set; } = string.Empty;
    public double Result { get; set; }
}
