using MongoDB.Bson;
using MongoDB.Driver;
using Posterr.Entities;

namespace Posterr.Repositories;

public class MongoDbPostsRepository : IPostsRepository
{
    private const string databaseName = "posterr";
    private const string collectionName = "posts";
    private readonly IMongoCollection<Post> postsCollection;
    private readonly FilterDefinitionBuilder<Post> filterBuilder = Builders<Post>.Filter;
    public MongoDbPostsRepository(IMongoClient mongoClient){
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        postsCollection = database.GetCollection<Post>(collectionName);
    }
    public async Task CreatePostAsync(Post post)
    {
        await postsCollection.InsertOneAsync(post);
    }

    public async Task<Post> GetPostAsync(Guid id)
    {
        var filter = filterBuilder.Eq(post => post.Id, id);
        return await postsCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        return await postsCollection.FindAsync(new BsonDocument()).Result.ToListAsync();
    }
}