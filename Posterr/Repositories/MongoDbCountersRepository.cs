using MongoDB.Driver;
using Posterr.Entities;
using Posterr.Filter;

namespace Posterr.Repositories;

public class MongoDbCountersRepository : ICountersRepository
{
    private const string databaseName = "posterr";
    private const string collectionName = "counters";
    private readonly IMongoCollection<Counter> countersCollection;
    private readonly FilterDefinitionBuilder<Counter> filterBuilder = Builders<Counter>.Filter;
    public MongoDbCountersRepository(IMongoClient mongoClient){
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        countersCollection = database.GetCollection<Counter>(collectionName);
    }

    public async Task<Counter> GetCounterAsync(string id)
    {
        var filter = filterBuilder.Eq(counter => counter.Id, id);
        return await countersCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task UpdateCounterAsync(string id)
    {
        var filter = filterBuilder.Eq(counter => counter.Id, id);
        // If i were using global variables, this get counter inside the update clause would be eliminated
        var update = Builders<Counter>.Update.Set("Value", GetCounterAsync(id).Result.Value + 1);
        var counter = await countersCollection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateCounterAsync(string id, long postCounter)
    {
        var filter = filterBuilder.Eq(counter => counter.Id, id);
        var update = Builders<Counter>.Update.Set("Value", postCounter);
        var counter = await countersCollection.UpdateOneAsync(filter, update);
    }
}