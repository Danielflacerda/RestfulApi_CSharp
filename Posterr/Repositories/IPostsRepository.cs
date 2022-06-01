using Posterr.Entities;

namespace Posterr.Repositories;
public interface IPostsRepository
{
    Task<Post> GetPostAsync(Guid id);
    Task<IEnumerable<Post>> GetPostsAsync();
    Task CreatePostAsync(Post post);
}