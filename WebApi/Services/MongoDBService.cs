namespace WebApi.Services;

public class MongoDBService(string connectionString, string databaseName)
{
    private readonly IMongoDatabase _database = new MongoClient(connectionString).GetDatabase(databaseName);

    public IMongoCollection<ConversationClients> ConversationsClients => _database.GetCollection<ConversationClients>("identified_customers");
    public IMongoCollection<ConversationNoClients> ConversationsNoClients => _database.GetCollection<ConversationNoClients>("anonymous_customers");
}
