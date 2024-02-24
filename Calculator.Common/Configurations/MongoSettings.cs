namespace Calculator.Common.Configurations;

public class MongoSettings
{
    public string ConnectionString { get; init; }
    public string ExpressionDatabaseName { get; init; }
    public string ExpressionCollectionName { get; init; }
    public string SagaDatabaseName { get; init; }
}
