using Posterr.Entities;

namespace Posterr.Repositories;
public interface IPostsRepository
{
    Post GetPost(long id);
    IEnumerable<Post> GetPosts();
    long GetMaxId();
    void CreatePost(Post post);
}