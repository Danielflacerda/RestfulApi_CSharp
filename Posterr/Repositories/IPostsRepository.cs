using Posterr.Entities;
using Posterr.Filter;

namespace Posterr.Repositories;
public interface IPostsRepository
{
    Task<Post> GetPostAsync(long id);
    Task<IEnumerable<Post>> GetPostsAsync(PaginationFilter pageNumber, string sessionUsername, bool filteredByFollowing);
    Task<IEnumerable<Post>> GetPostsUserPageAsync(string username, PaginationFilter pageNumber);
    Task CreatePostAsync(Post post);
    Task<long> GetTodayUserPostsCounterAsync(string username);
}