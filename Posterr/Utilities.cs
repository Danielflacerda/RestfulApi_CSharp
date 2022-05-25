using Posterr.Dtos;
using Posterr.Entities;

namespace Posterr;
public static class Utilities{

    public static PostDto PostAsDto(this Post post){
        return new PostDto{
                PostId = post.PostId,
                Content = post.Content,
                RepostedPostId = post.RepostedPostId,
                PostedByUserId = post.PostedByUserId,
                CreatedOn = post.CreatedOn
        };
    }
}