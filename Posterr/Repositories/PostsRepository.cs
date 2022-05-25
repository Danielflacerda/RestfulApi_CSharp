using System.Collections.Generic;
using Posterr.Entities;

namespace Posterr.Repositories;

public class PostsRepository : IPostsRepository
{
    private readonly List<Post> posts = new(){
        new Post {
            PostId = 1, Content = "Hello World!", PostedByUserId = 1, CreatedOn = DateTime.Now
        }
    };

    public IEnumerable<Post> GetPosts()
    {
        return posts;
    }

    public Post GetPost(long id)
    {
        return posts.Where(post => post.PostId == id).SingleOrDefault();
    }

    public void CreatePost(Post post)
    {
        posts.Add(post);
    }

    public long GetMaxId()
    {
        return posts.OrderByDescending(post => post.PostId).FirstOrDefault().PostId;
    }
}