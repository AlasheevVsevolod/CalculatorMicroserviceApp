using MongoDB.Bson.Serialization.Attributes;

namespace Calculator.AdditionService.Models;

public class AdditionDocumentModel
{
    [BsonId]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Expression { get; set; } = string.Empty;
    public double Result { get; set; }
    public DateTime UpdatedOn = DateTime.UtcNow;
}
