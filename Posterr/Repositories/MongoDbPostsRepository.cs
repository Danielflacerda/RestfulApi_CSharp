using MongoDB.Driver;
using Posterr.Entities;

namespace Posterr.Repositories;

public class MongoDbPostsRepository : IPostsRepository
{
    private const string databaseName = "posterr";
    private const string collectionName = "posts";
    private readonly IMongoCollection<Post> postsCollection;
    public MongoDbPostsRepository(IMongoClient mongoClient){
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        postsCollection = database.GetCollection<Post>(collectionName);
    }
    public void CreatePost(Post post)
    {
        postsCollection.InsertOne(post);
    }

    public long GetMaxId()
    {
        throw new NotImplementedException();
    }

    public Post GetPost(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Post> GetPosts()
    {
        throw new NotImplementedException();
    }
}