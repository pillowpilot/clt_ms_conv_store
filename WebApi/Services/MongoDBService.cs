namespace WebApi.Services;

public class MongoDBService(string connectionString, string databaseName)
{
    private readonly IMongoDatabase _database = new MongoClient(connectionString).GetDatabase(databaseName);

    public IMongoCollection<IdentifiedCustomer> IdentifiedCustomers => _database.GetCollection<IdentifiedCustomer>("identified_customers");
    public IMongoCollection<AnonymousCustomer> AnonymousCustomers => _database.GetCollection<AnonymousCustomer>("anonymous_customers");
}
