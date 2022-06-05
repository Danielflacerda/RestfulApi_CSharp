using Posterr.Dtos;
using Posterr.Entities;

namespace Posterr;
public static class Utilities{

    public static PostDto PostAsDto(this Post post){
        return new PostDto{
                Id = post.Id,
                Content = post.Content,
                RepostedPostId = post.RepostedPostId,
                PostedByUsername = post.PostedByUsername,
                CreatedOn = post.CreatedOn
        };
    }
    public static UserDto UserAsDto(this User user){
        return new UserDto{
                Id = user.Id,
                Username = user.Username,
                JoinedDate = user.CreatedOn.ToString("MMMM dd, yyyy"),
                Followers = user.Followers.Count(),
                Following = user.Following.Count(),
                PostsCount = user.PostsCount
        };
    }
}