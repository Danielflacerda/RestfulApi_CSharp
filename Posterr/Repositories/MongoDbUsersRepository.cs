using MongoDB.Bson;
using MongoDB.Driver;
using Posterr.Entities;

namespace Posterr.Repositories;

public class MongoDbUsersRepository : IUsersRepository
{
    public DateTime? lastUserRecovered = null;
    private const string databaseName = "posterr";
    private const string collectionName = "users";
    private readonly IMongoCollection<User> usersCollection;
    private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;
    public MongoDbUsersRepository(IMongoClient mongoClient){
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        usersCollection = database.GetCollection<User>(collectionName);
    }

    public async Task<User> GetUserAsync(string userName)
    {
        var filter = filterBuilder.Eq(user => user.Username, userName);
        return await usersCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = await usersCollection.Find(_ => true).ToListAsync();
        lastUserRecovered = null;
        return users;
    }

    public async Task UpdateUserPostCounter(string username)
    {
        var filter = filterBuilder.Eq(user => user.Username, username);
        // If i were using session control, using session user object would eliminate this get user inside the update clause.
        var update = Builders<User>.Update.Set("Value", GetUserAsync(username).Result.PostsCount + 1);
        await usersCollection.UpdateOneAsync(filter, update);
    }
}