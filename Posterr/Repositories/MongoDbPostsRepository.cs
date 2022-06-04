using MongoDB.Bson;
using MongoDB.Driver;
using Posterr.Entities;
using Posterr.Filter;

namespace Posterr.Repositories;

public class MongoDbPostsRepository : IPostsRepository
{
    // If i had a session control i would use a sessionUser variable for when current logged user data is necessary,
    // because of that i'm going to hardcode current user so the api methods works as it should.
    public User sessionUser = new User{
        Id = 1,
        Username = "TheJoshua",
        CreatedOn = DateTime.Parse("01/01/2015"),
        Followers = new List<string>{"Biden", "Obama", "Trump"},
        Following = new List<string>{"Biden", "Obama", "Trump", "JhonnyUchiha"},
        PostsCount = 12
    };
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

    public async Task<Post> GetPostAsync(long id)
    {
        var filter = filterBuilder.Eq(post => post.Id, id);
        return await postsCollection.Find(filter).SingleOrDefaultAsync();
    }    
    
    public async Task<long> GetTodayUserPostsCounterAsync(string username)
    {
        var filter = filterBuilder.Eq(post => post.PostedByUsername, username);
        filter &= filterBuilder.Gt(post => post.CreatedOn, DateTime.Today);
        return await postsCollection.Find(filter).CountDocumentsAsync();
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(PaginationFilter filter, bool filteredByFollowing)
    {
        if(filteredByFollowing)
        {
            var queryFilter = filterBuilder.In(x => x.PostedByUsername, sessionUser.Following);
            var posts = await postsCollection.Find(queryFilter)
                .SortByDescending(x => x.CreatedOn)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .ToListAsync();
            return posts;
        }
        else
        {
            var posts = await postsCollection.Find(_ => true)
                .SortByDescending(x => x.CreatedOn)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .ToListAsync();
            return posts;
        }
    }
    public async Task<IEnumerable<Post>> GetPostsUserPageAsync(string username, PaginationFilter paginationFilter)
    {
        var queryFilter = filterBuilder.Eq(x => x.PostedByUsername, username);
        var posts = await postsCollection.Find(queryFilter)
            .SortByDescending(x => x.CreatedOn)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Limit(paginationFilter.PageSize)
            .ToListAsync();
        return posts;
    }
}