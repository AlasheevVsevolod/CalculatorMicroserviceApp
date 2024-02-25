using MongoDB.Bson.Serialization.Attributes;

namespace SubtractionService.Models;

public class SubtractionDocumentModel
{
    [BsonId]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Expression { get; set; } = string.Empty;
    public double Result { get; set; }
    public DateTime UpdatedOn = DateTime.UtcNow;
}
