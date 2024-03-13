using MongoDB.Bson.Serialization.Attributes;

namespace Calculator.DivisionService.Models;

public class DivisionDocumentModel
{
    [BsonId]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Expression { get; set; } = string.Empty;
    public double Result { get; set; }
    public DateTime UpdatedOn = DateTime.UtcNow;
}
