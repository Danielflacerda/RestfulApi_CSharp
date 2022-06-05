using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Posterr.Entities;
using Posterr.Filter;

namespace Posterr.Repositories;

public class MongoDbPostsRepository : IPostsRepository
{
    // If i had a session control i would use a sessionUser variable for when current logged user data is necessary,
    // because of that i'm going to hardcode current user so the api methods works as it should.
    private const string databaseName = "posterr";
    private const string collectionName = "posts";
    private readonly IMongoCollection<Post> postsCollection;
    private readonly IUsersRepository _usersRepository;
    private readonly FilterDefinitionBuilder<Post> filterBuilder = Builders<Post>.Filter;
    public MongoDbPostsRepository(IMongoClient mongoClient, IUsersRepository usersRepository){
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        postsCollection = database.GetCollection<Post>(collectionName);
        _usersRepository = usersRepository;
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

    public async Task<IEnumerable<Post>> GetPostsAsync(PaginationFilter filter, string sessionUsername, bool filteredByFollowing)
    {
        if(filteredByFollowing)
        {
            var queryFilter = filterBuilder.In(x => x.PostedByUsername, _usersRepository.GetUserAsync(sessionUsername).Result.Following);
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

    public async Task<IEnumerable<Post>> SearchAsync(string searchContent, PaginationFilter paginationFilter)
    {
        var queryFilter = filterBuilder.Regex( "Content", new BsonRegularExpression("/"+searchContent+"/"));
        queryFilter &= filterBuilder.Ne( x => x.Content, "");
        queryFilter &= filterBuilder.Ne( x => x.Content, null);
        var posts = await postsCollection.Find(queryFilter)
            .SortByDescending(x => x.CreatedOn)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Limit(paginationFilter.PageSize)
            .ToListAsync();
        return posts;
    }
}